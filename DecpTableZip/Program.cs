using System;
using System.Collections.Generic;
using System.Data.HashFunction.xxHash;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Program
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


    static void Main(string[] args)
    {
        string key = "Excel.zip";
        int length = 20;

        string password = CreatePassword(key, length);
        Console.WriteLine(password);
    }
}
