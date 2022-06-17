using Models.File;
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

        private async Task<IOResult> sendMessage(AugmentedOutputStream output, Type type, object message)
        {
            string typeString = type.FullName;
            var metadata = new ServerMessageMetadata(typeString);

            return await output.WriteObjectsAsync(metadata, message);
        }

        private Task sendMessageToAll(IEnumerable<AugmentedOutputStream> outputs, Type messageType, object message)
        {
            List<Task> taskList = new List<Task>();
            foreach (var output in outputs)
            {
                Task<IOResult> sendTask = sendMessage(output, messageType, message);
                taskList.Add(sendTask);
            }

            return Task.WhenAll(taskList);
        }

        public Task SendModListUpdateToAll(IEnumerable<AugmentedOutputStream> outputs, List<ModInfo> mods)
        {
            var message = new ModListUpdateMessage(mods);
            return sendMessageToAll(outputs, typeof(ModListUpdateMessage), message); 
        }

        public Task SendUserListUpdateToAll(IEnumerable<AugmentedOutputStream> outputs, List<UserInfo> users)
        {
            var message = new UserListUpdateMessage(users);
            return sendMessageToAll(outputs, typeof(UserListUpdateMessage), message);
        }

        public Task SendFileReadyMessageToAll(IEnumerable<AugmentedOutputStream> outputs, FileDescriptor fd, FileType ft)
        {
            var message = new FileReadyMessage(fd, ft);
            return sendMessageToAll(outputs, typeof(FileReadyMessage), message);
        }

        public async Task<IOResult<MessageTypePair>> RecieveMessage(AugmentedInputStream input)
        {
            throw new NotImplementedException();
        }        

    }
}
