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

        private async Task<IOResult> sendMessage(AugmentedOutputStream output, Type type, object message)
        {
            string typeString = type.FullName;
            var metadata = new ClientMessageMetadata(typeString);

            return await output.WriteObjectsAsync(metadata, message);
        }

        public async Task<IOResult> SendModAdd(AugmentedOutputStream output, ModInfo mod)
        {
            var message = new ModAddMessage(mod);
            return await sendMessage(output, typeof(ModAddMessage), message);
        }

        public async Task<IOResult> SendUserInfoUpdate(AugmentedOutputStream output, UserInfo user)
        {
            var message = new UserUpdateMessage(user);
            return await sendMessage(output, typeof(UserUpdateMessage), message);
        }

        public async Task<IOResult<MessageTypePair>> RecieveMessage(AugmentedInputStream input)
        {
            var metadataResult = await input.ReadObjectAsync<ServerMessageMetadata>();
            if (metadataResult.Failed)
                return IOResult.CreateFailure<MessageTypePair>(metadataResult.ErrorType);

            string typeString = metadataResult.Value.MessageType;
            Type? type = Type.GetType(typeString);
            if (type == null)
                return IOResult.CreateFailure<MessageTypePair>(IOErrorType.ImproperFormat);

            var messageResult = await input.ReadObjectAsync<object>(type);
            if(messageResult.Failed)
                return IOResult.CreateFailure<MessageTypePair>(messageResult.ErrorType);

            var messageTypePair = new MessageTypePair(type, messageResult.Value);

            return IOResult.CreateSuccess(messageTypePair);
        }

    }
}
