using Models.Network;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostLib
{
    public class HostMessageChanel
    {

        public async Task<IOResult> SendMessage(AugmentedOutputStream output, Type type, object message)
        {
            string typeString = type.FullName;
            var metadata = new ServerMessageMetadata(typeString);

            return await output.WriteObjectsAsync(metadata, message);
        }

        public async Task<IOResult<TypeMessagePair>> RecieveMessage(AugmentedInputStream input)
        {
            throw new NotImplementedException();
        }        

    }
}
