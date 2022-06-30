using ClientLib.Models;
using Models.Network;
using Models.Network.Messages.Client;
using Models.Network.Messages.Server;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientLib
{
    public class MessageChanel
    {

        public async Task<IOResult> SendMessage(AugmentedOutputStream output, Type type, object message)
        {
            string typeString = type.FullName;
            var metadata = new ClientMessageMetadata(typeString);

            return await output.WriteObjectsAsync(metadata, message);
        }

        public async Task<IOResult<TypeMessagePair>> RecieveMessage(AugmentedInputStream input)
        {
            var metadataResult = await input.ReadObjectAsync<ServerMessageMetadata>();
            if (metadataResult.Failed)
                return IOResult.CreateFailure<TypeMessagePair>(metadataResult.ErrorType);

            string typeString = metadataResult.Value.MessageType;
            Type? type = Type.GetType(typeString);
            if (type == null)
                return IOResult.CreateFailure<TypeMessagePair>(IOErrorType.ImproperFormat);

            var messageResult = await input.ReadObjectAsync<object>(type);
            if(messageResult.Failed)
                return IOResult.CreateFailure<TypeMessagePair>(messageResult.ErrorType);

            var messageTypePair = new TypeMessagePair(type, messageResult.Value);

            return IOResult.CreateSuccess(messageTypePair);
        }

    }
}
