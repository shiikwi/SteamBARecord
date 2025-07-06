using System;
using System.Data.HashFunction.xxHash;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace DecpTableZip
{
    class TableService
    {
        public static string CreatePassword(string key, int length)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            var xxh32 = xxHashFactory.Instance.Create(new xxHashConfig { HashSizeInBits = 32, Seed = 0 });
            var hashBytes = xxh32.ComputeHash(keyBytes).Hash;
            var seed = BitConverter.ToUInt32(hashBytes, 0);

            var mt = new MersenneTwister(seed);

            int targetByteLength = (3 * length) >> 2;
            var resultBytes = new List<byte>(targetByteLength);

            while (resultBytes.Count < targetByteLength)
            {
                int randomInt = mt.Next();

                byte[] bytesN = BitConverter.GetBytes(randomInt);

                resultBytes.AddRange(bytesN);
            }

            byte[] finalBytes = resultBytes.Take(targetByteLength).ToArray();
            return Convert.ToBase64String(finalBytes);
        }
    }

    class Program
    {
        const int length = 20;

        static void Main(string[] args)
        {
            if (args.Length != 1 || !Directory.Exists(args[0]))
            {
                Console.WriteLine("Usage: DecpTableZip.exe <inputfloder>");
                return;
            }

            string inputFolder = args[0];
            string[] zipfiles = Directory.GetFiles(inputFolder, "*.zip", SearchOption.TopDirectoryOnly);
            if (zipfiles.Length == 0)
            {
                Console.WriteLine("Zip file Not Exist");
                return;
            }

            string outputDir = Path.Combine(inputFolder, "decompress");
            Directory.CreateDirectory(outputDir);

            foreach (var zip in zipfiles)
            {
                var fileName = Path.GetFileName(zip);
                var password = TableService.CreatePassword(fileName, length);
                try
                {
                    var options = new ReaderOptions
                    {
                        Password = password,
                        ArchiveEncoding = new ArchiveEncoding
                        {
                            Password = Encoding.UTF8
                        }
                    };
                    using var archive = ArchiveFactory.Open(zip, options);

                    foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                    {
                        entry.WriteToDirectory(outputDir, new ExtractionOptions
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }

                    Console.WriteLine($"Decompressed: {fileName} Password: {password}");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Decompress {fileName} Failed {ex.Message}");
                }

            }

        }

    }


}
