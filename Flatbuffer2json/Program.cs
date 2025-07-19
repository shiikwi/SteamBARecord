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
using System.Data.SQLite;

namespace Flatbuffer2json
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            //EncryptBytesFile dump
            //Change filename to what you want to dump(lower case)

            string inputBytesFile = "academyfavorscheduleexceltable.bytes";
            string outputfile = Path.GetFileNameWithoutExtension(inputBytesFile) + ".json";

            var keymap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("BytesKey.json"));
            var nameraw = keymap[inputBytesFile];
            var bytes = File.ReadAllBytes(inputBytesFile);

            var converter = new EncryptTableConvert();
            var jsonstring = converter.ConverToJson(nameraw, bytes);

            File.WriteAllText(outputfile, jsonstring);
            Console.WriteLine($"Successfully dump {inputBytesFile} to {outputfile}");
#else
            //DbBytes dump
            //Change dbtablename to tablename from sqlite db

            string dbPath = "./ExcelDB.db";
            string dbtablename = "VoiceDBSchema";

            var ouputfile = $"{dbtablename}.json";
            var converter = new DbTableConvert();
            using (var connection = new SQLiteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                string query = $"SELECT * FROM {dbtablename}";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    var alldata = new List<dynamic>();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = dbtablename.Replace("DBSchema", "Excel");
                            byte[] bytes = (byte[])reader["Bytes"];

                            string jsonstring = converter.ConverToJson(tableName, bytes);
                            var rowstring = JsonConvert.DeserializeObject<dynamic>(jsonstring);
                            alldata.Add(rowstring);
                        }

                        string jsonout = JsonConvert.SerializeObject(alldata, Formatting.Indented);
                        File.WriteAllText(ouputfile, jsonout);
                        Console.WriteLine($"Successfully dump {dbtablename} to {ouputfile}");
                    }
                }

            }

#endif
        }

    }
}
