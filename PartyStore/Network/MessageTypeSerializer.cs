using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PartyStore.Network
{
    internal class MessageTypeSerializer
    {

        public static string Serialize(Type type)
        {
            return type.Name;
        }

        public static Type? Deserialize(string typeName)
        {
            return Type.GetType(typeName);
        }

    }
}
