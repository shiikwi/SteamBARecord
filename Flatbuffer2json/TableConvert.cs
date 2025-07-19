using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Google.FlatBuffers;
using Newtonsoft.Json;

namespace Flatbuffer2json
{
    public class EncryptTableConvert : TableDecryptionService
    {
        public override byte[] GetXorKey(string tableName)
        {
            var name = tableName.Replace("ExcelTable", "");

            return CreateKey(name);
        }

    }
    public class DbTableConvert : TableDecryptionService
    {
        public override byte[] GetXorKey(string tableName)
        {
            return null;
        }
    }
}
