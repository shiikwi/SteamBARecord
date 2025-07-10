using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Data.HashFunction.xxHash;

namespace Decryptbytes
{
    public class TableEncryptionService
    {
        public static byte[] XOR(string name, byte[] bytes)
        {
            byte[] keybytes = Encoding.UTF8.GetBytes(name);
            var xxh32 = xxHashFactory.Instance.Create(new xxHashConfig { HashSizeInBits = 32, Seed = 0 });
            byte[] hashbyte = xxh32.ComputeHash(keybytes).Hash;
            var seed = BitConverter.ToUInt32(hashbyte, 0);

            var mt = new MersenneTwister(seed);

            byte[] randomByte = new byte[4];
            int randomByteIndex = 4;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (randomByteIndex >= 4)
                {
                    int randomInt = mt.Next();
                    randomByte = BitConverter.GetBytes(randomInt);
                    randomByteIndex = 0;
                }

                bytes[i] ^= randomByte[randomByteIndex];
                randomByteIndex++;
            }

            return bytes;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string KeyMapFile = "BytesKey.json";
            string inputfolder = args[0];
            string ouputfolder = Path.Combine(inputfolder, "decrypt");
            if (!Directory.Exists(ouputfolder))
            {
                Directory.CreateDirectory(ouputfolder);
            }

            try
            {
                if (args.Length != 1 || !Directory.Exists(inputfolder))
                {
                    Console.WriteLine("Usage: Decryptbytes.exe <inputBytesFolder>");
                    return;
                }

                if (!File.Exists(KeyMapFile))
                {
                    Console.WriteLine("Use GenXORNamelist.py to generate BytesKey.json");
                    return;
                }

                var keyMap = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(KeyMapFile));
                var fileEncrypt = Directory.GetFiles(inputfolder, "*.bytes");

                foreach (var filepath in fileEncrypt)
                {
                    var filename = Path.GetFileName(filepath);
                    if (keyMap.TryGetValue(filename, out string decryptkey))
                    {
                        byte[] encryptdata = File.ReadAllBytes(filepath);
                        byte[] decryptdata = TableEncryptionService.XOR(decryptkey, encryptdata);
                        var outputfile = Path.Combine(ouputfolder, filename);
                        File.WriteAllBytes(outputfile, decryptdata);
                        Console.WriteLine($"Decrypted {filename}");
                    }
                    else
                    {
                        Console.WriteLine($"Cannot find {filename} key");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Decrypt Failed: {ex.Message}");
            }
        }
    }
}