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
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: Flatbuffer2json.exe <bytes/db> <path to bytes folder or db file>");
                return;
            }

            string mode = args[0].ToLower();
            string path = args[1];

            try
            {
                if (mode == "bytes")
                {
                    ProcessBytes(path);
                }
                else if (mode == "db")
                {
                    Processdb(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ProcessBytes(string path)
        {
            //BytesFile dump
            string outFolder = Path.Combine(path, "bytes2json");
            if (!Directory.Exists(outFolder)) Directory.CreateDirectory(outFolder);

            var files = Directory.GetFiles(path, "*.bytes");
            var keymap = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("BytesKey.json"));
            var converter = new EncryptTableConvert();

            foreach (var inputBytesFile in files)
            {
                string outputfile = Path.Combine(outFolder, Path.GetFileNameWithoutExtension(inputBytesFile) + ".json");
                var nameraw = keymap[Path.GetFileName(inputBytesFile)];
                var bytes = File.ReadAllBytes(inputBytesFile);
                var jsonstring = converter.ConverToJson(nameraw, bytes);
                File.WriteAllText(outputfile, jsonstring);

                Console.WriteLine($"Successfully dump {Path.GetFileName(inputBytesFile)}");
            }
        }

        static void Processdb(string path)
        {
            //DbBytes dump
            string outFolder = Path.Combine(Path.GetDirectoryName(path), "db2json");
            if (!Directory.Exists(outFolder)) Directory.CreateDirectory(outFolder);

            var converter = new DbTableConvert();
            using (var connection = new SQLiteConnection($"Data Source={path}"))
            {
                connection.Open();

                var tableNames = new List<string>();
                string tablequery = "SELECT name FROM sqlite_master WHERE type='table'";
                using (SQLiteCommand cmd = new SQLiteCommand(tablequery, connection))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0);
                            tableNames.Add(tableName);
                        }
                    }
                }

                foreach (var dbtablename in tableNames)
                {
                    string tableName = dbtablename.Replace("DBSchema", "Excel");
                    string outputfile = Path.Combine(outFolder, tableName + ".json");

                    var alldata = new List<dynamic>();
                    string query = $"SELECT * FROM {dbtablename}";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                byte[] bytes = (byte[])reader["Bytes"];

                                string jsonstring = converter.ConverToJson(tableName, bytes);
                                var rowstring = JsonConvert.DeserializeObject<dynamic>(jsonstring);
                                alldata.Add(rowstring);
                            }

                            string jsonout = JsonConvert.SerializeObject(alldata, Formatting.Indented);
                            File.WriteAllText(outputfile, jsonout);
                            Console.WriteLine($"Successfully dump {dbtablename}");
                        }
                    }
                }

            }
        }

    }
}
