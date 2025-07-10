using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FlatData;
using Google.FlatBuffers;
using Newtonsoft.Json;

namespace Flatbuffer2json
{
    public class TableEncryptionService
    {
        public static byte[] CreateKey(string name)
        {
            const int keylength = 8;
            byte[] keybytes = Encoding.UTF8.GetBytes(name);

            var xxh32 = System.Data.HashFunction.xxHash.xxHashFactory.Instance.Create(new System.Data.HashFunction.xxHash.xxHashConfig { HashSizeInBits = 32, Seed = 0 });
            byte[] hashbytes = xxh32.ComputeHash(keybytes).Hash;
            var seed = BitConverter.ToUInt32(hashbytes, 0);

            var mt = new Decryptbytes.MersenneTwister(seed);
            var resultbytes = new List<byte>(keylength);
            while (resultbytes.Count < keylength)
            {
                int randomInt = mt.Next();
                byte[] byteN = BitConverter.GetBytes(randomInt);
                resultbytes.AddRange(byteN);
            }
            return resultbytes.Take(keylength).ToArray();
        }

        public static T TableConvert<T>(T value, byte[] key) where T : struct
        {
            if (key == null || key.Length < 8) return value;
            if (Convert.ToInt64(value) == 0) return value;

            if (value is long vallong) return (T)(object)(vallong ^ BitConverter.ToInt64(key, 0));
            if (value is ulong valulong) return (T)(object)(valulong ^ BitConverter.ToUInt64(key, 0));
            if (value is int valint) return (T)(object)(valint ^ BitConverter.ToInt32(key, 0));
            if (value is uint valuint) return (T)(object)(valuint ^ BitConverter.ToUInt32(key, 0));

            if (value is float valfloat)
            {
                int intRep = BitConverter.ToInt32(BitConverter.GetBytes(valfloat), 0);
                int keypart = BitConverter.ToInt32(key, 0);
                return (T)(object)BitConverter.ToSingle(BitConverter.GetBytes(intRep ^ keypart), 0);
            }

            if (value is bool valbool)
            {
                int boolint = valbool ? 1 : 0;
                int keypart = BitConverter.ToInt32(key, 0);
                return (T)(object)((boolint ^ keypart) != 0);
            }

            if (value is Enum)
            {
                var underlyingType = Enum.GetUnderlyingType(typeof(T));
                var numericValue = Convert.ChangeType(value, underlyingType);

                var convertMethod = typeof(TableEncryptionService).GetMethod("TableConvert").MakeGenericMethod(underlyingType);
                var decryptedNumericValue = convertMethod.Invoke(null, new object[] { numericValue, key });

                return (T)Enum.ToObject(typeof(T), decryptedNumericValue);
            }


            return value;
        }

        public static string StringConvert(string value, byte[] key)
        {
            if (string.IsNullOrEmpty(value) || key == null || key.Length == 0) return value;

            try
            {
                byte[] data = Convert.FromBase64String(value);
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] ^= key[i % key.Length];
                }

                return Encoding.Unicode.GetString(data);
            }
            catch (FormatException)
            {
                return value;
            }
        }
    }


    class Program
    {
        public static dynamic ConvertFlatBuffer(IFlatbufferObject fbobj, byte[] xorkey)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;
            var type = fbobj.GetType();

            try
            {
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    string propName = property.Name;

                    if (propName.StartsWith("__") || propName == "ByteBuffer")
                    {
                        continue;
                    }

                    //Handle Method
                    if (propName.EndsWith("Length"))
                    {
                        string baseName = propName.Substring(0, propName.Length - "Length".Length);

                        MethodInfo VecMethod = type.GetMethod(baseName, new[] { typeof(int) });

                        if (VecMethod == null) continue;

                        int length = (int)property.GetValue(fbobj);
                        var list = new List<object>();

                        if (length > 0)
                        {
                            Type itemType = VecMethod.ReturnType;
                            bool isValueType = itemType.IsValueType;
                            bool isString = itemType == typeof(string);
                            MethodInfo decryptMethod = null;

                            if (isValueType && (itemType.IsPrimitive || itemType.IsEnum))
                            {
                                decryptMethod = typeof(TableEncryptionService).GetMethod("TableConvert").MakeGenericMethod(itemType);
                            }

                            for (int i = 0; i < length; i++)
                            {
                                object encryptedValue = VecMethod.Invoke(fbobj, new object[] { i });
                                object decryptedValue = encryptedValue;

                                if (isString && encryptedValue is string strItem)
                                {
                                    decryptedValue = TableEncryptionService.StringConvert(strItem, xorkey);
                                }
                                else if (decryptMethod != null)
                                {
                                    decryptedValue = decryptMethod.Invoke(null, new object[] { encryptedValue, xorkey });
                                }

                                if (itemType.IsEnum)
                                {
                                    list.Add(decryptedValue.ToString());
                                }
                                else
                                {
                                    list.Add(decryptedValue);
                                }
                            }
                        }
                        expando[baseName] = list;
                    }

                    //Handle Proterty
                    else if (property.GetIndexParameters().Length == 0)
                    {
                        if (expando.ContainsKey(propName)) continue;

                        object encryptedValue = property.GetValue(fbobj);
                        object decryptedValue = encryptedValue;

                        Type propType = property.PropertyType;

                        if (encryptedValue is string value)
                        {
                            decryptedValue = TableEncryptionService.StringConvert(value, xorkey);
                        }
                        else if (decryptedValue != null && propType.IsValueType && (propType.IsPrimitive || propType.IsEnum))
                        {
                            var decryptMethod = typeof(TableEncryptionService).GetMethod("TableConvert").MakeGenericMethod(propType);
                            decryptedValue = decryptMethod.Invoke(null, new object[] { encryptedValue, xorkey });
                        }

                        if (propType.IsEnum)
                        {
                            expando[propName] = decryptedValue.ToString();
                        }
                        else
                        {
                            expando[propName] = decryptedValue;
                        }
                    }
                }
                return expando;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reflection Error for type {type.Name}: {ex.Message}\n{ex.StackTrace}");
                return expando;
            }
        }

        public static Type FindType(string fullTypeName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(fullTypeName, false);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        static void Main(string[] args)
        {
            //Change name to what you want to dump(lowercase)
            string inputBytesFile = "academyfavorscheduleexceltable.bytes";
            string fileName = Path.GetFileNameWithoutExtension(inputBytesFile);
            string outputFile = fileName + ".json";

            var keymap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("BytesKey.json"));
            var nameraw = keymap[inputBytesFile];
            var name = nameraw.Replace("ExcelTable", "");
            var xorkey = TableEncryptionService.CreateKey(name);

            byte[] fileBytes = File.ReadAllBytes(inputBytesFile);
            var bytebuffer = new Google.FlatBuffers.ByteBuffer(fileBytes);

            string tableTypeName = $"FlatData.{nameraw}";
            Type tableType = FindType(tableTypeName);
            if (tableType == null)
            {
                Console.WriteLine($"Error: Type '{tableTypeName}' not found");
                return;
            }

            MethodInfo getRootMethod = tableType.GetMethod("GetRootAs" + nameraw, new[] { typeof(ByteBuffer) });
            IFlatbufferObject table = (IFlatbufferObject)getRootMethod.Invoke(null, new object[] { bytebuffer });

            PropertyInfo dataListLengthProp = tableType.GetProperty("DataListLength");
            MethodInfo dataListMethod = tableType.GetMethod("DataList");

            int rowcount = (int)dataListLengthProp.GetValue(table);
            var dyList = new List<dynamic>();

            Console.WriteLine($"Processing {rowcount} rows from {inputBytesFile}");

            for (int i = 0; i < rowcount; i++)
            {
                object nullableRow = dataListMethod.Invoke(table, new object[] { i });
                if (nullableRow != null)
                {
                    IFlatbufferObject fbRow = (IFlatbufferObject)nullableRow;

                    dynamic dyRow = ConvertFlatBuffer(fbRow, xorkey);
                    dyList.Add(dyRow);
                }
            }

            string jsonstring = JsonConvert.SerializeObject(dyList, Formatting.Indented);
            File.WriteAllText(outputFile, jsonstring);
            Console.WriteLine($"Successfully converted {inputBytesFile} to {outputFile}");
        }
    }
}