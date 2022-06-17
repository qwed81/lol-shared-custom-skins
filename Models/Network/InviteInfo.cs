using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Models.Network
{

    public static class InviteInfoParams
    {

        public static IPAddress? LocalIP { get; set; }

        public static IPAddress? PublicIP { get; set; }
        
        public static int? Port { get; set; }

    }

    public class InviteInfo
    {

        public string Password { get; }

        public string LocalIP { get; }

        public string PublicIP { get; }

        public Guid SessionId { get; }

        public int Port { get; }

        public bool Admin { get; }

        private InviteInfo(string localIp, string publicIp, int port, string password, Guid sessionId)
        {
            Password = password;
            LocalIP = localIp;
            PublicIP = publicIp;
            Port = port;
            SessionId = sessionId;
        }

        public static InviteInfo CreateInvite(Guid sessionId, string? password = null)
        {
            if(password == null)
            {
                Random random = new Random();
                Span<char> chars = stackalloc char[16];
                for (int i = 0; i < chars.Length; i++)
                    chars[i] = (char)random.Next(65, 90);
                password = new string(chars);
            }

            if (InviteInfoParams.LocalIP == null || InviteInfoParams.PublicIP == null || InviteInfoParams.Port == null)
                throw new Exception("Invite info params not set");

            string localIp = InviteInfoParams.LocalIP.ToString();
            string publicIp = InviteInfoParams.PublicIP.ToString();
            return new InviteInfo(localIp, publicIp, InviteInfoParams.Port.Value, password, sessionId);
        }
        
    }
}
