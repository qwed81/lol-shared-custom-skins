using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

namespace StoreModels
{
    public class SerializeUtil
    {

        public static string TypeToString(Type type)
        {
            return type.Name;
        }

        public static Type? StringToType(string typeName)
        {
            return Type.GetType(typeName);
        }

        public static ulong ULongFromBytes(byte[] arr, int start)
        {
            Span<byte> byteSpan = new Span<byte>(arr, start, 8);
            if (BitConverter.IsLittleEndian != true) // makes sure this number is uniform across all devices
                byteSpan.Reverse();

            return BitConverter.ToUInt64(byteSpan);
        }

        public static void BytesFromULong(ulong value, byte[] buffer, int start)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian != true)
                bytes.Reverse();

            bytes.CopyTo(buffer, start);
        }

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static object? DeserializeObject(Type type, string? json)
        {
            if (json == null)
                return null;

            return JsonConvert.DeserializeObject(json, type);
        }



    }
}
