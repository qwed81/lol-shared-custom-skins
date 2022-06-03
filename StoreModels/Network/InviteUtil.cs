using System;
using System.Collections.Generic;
using System.Text;
using System.Net;


namespace StoreModels
{
    public class InviteUtil
    {

        public static string CreateInviteString(IPEndPoint endpoint, bool admin)
        {
            Random random = new Random();
            Span<char> chars = stackalloc char[16];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)random.Next(65, 90);
            string password = new string(chars);

            return endpoint.Address.ToString() + ":" + endpoint.Port + "/" + password;
        }

        public static (IPEndPoint, string) ParseInviteString(string str)
        {
            string[] strs = str.Split(':', '/');
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(strs[0]), Convert.ToInt32(strs[1]));
            return (endpoint, strs[2]);
        }

    }
}
