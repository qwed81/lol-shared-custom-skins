using Models.Network;
using Models.Network.Messages.Client;
using SCSClient.NetworkClient;
using SCSClient.Types;

namespace SCSClient.MessageTranslator
{
    public class ActionHandler
    {

        private LocalModLibrary _modLibrary;
        private EventStore _eventStore;
        private Client _client;

        public ActionHandler(LocalModLibrary library, EventStore eventStore, Client client)
        {
            _modLibrary = library;
            _eventStore = eventStore;
            _client = client;
        }

        public TypeMessagePair? HandleAction(UIAction action)
        {
            Func<UIAction, TypeMessagePair?> handler = action.type switch
            {
                UIActionType.ApplyChanges => apply,
                UIActionType.ChangeActivation => changeActivation,
                UIActionType.CreateInvite => createInvite,
                UIActionType.JoinParty => joinParty,
                UIActionType.UpdateUser => updateUser,
                UIActionType.LeaveParty => leaveParty,
                _ => throw new ArgumentException()
            };

            return handler(action);
        }

        private TypeMessagePair? apply(UIAction action)
        {
            // apply
            throw new NotImplementedException();
        }

        private TypeMessagePair? changeActivation(UIAction action)
        {
            var caAction = (ChangeActivationAction)action;
            _modLibrary.ChangeActivation(caAction.fileHash, caAction.value); // for responsivness
            var modTuple = _modLibrary[caAction.fileHash];
            var mod = modTuple.Item1;

            var reactMod = new ReactMod(mod.Description, mod.Name, mod.Author, mod.ProviderName,
                "", mod.FileHash, 100, modTuple.Item2);

            _eventStore.QueueDataUpdate(EventUser.ActionThread ,new SingleModDataUpdate(reactMod)); // small optimization

            var message = new ModActivationMessage(_modLibrary[caAction.fileHash].Item1, caAction.value);
            return new TypeMessagePair(typeof(ModActivationMessage), message);
        }

        private TypeMessagePair? createInvite(UIAction action)
        {
            throw new NotImplementedException();
        }

        private TypeMessagePair? joinParty(UIAction action)
        {
            _eventStore.QueueDataUpdate(EventUser.ActionThread, new PartyLoadingDataUpdate());

            var jpAction = (JoinPartyAction)action;
            _client.Connect(jpAction.invite);

            return null;
        }

        private TypeMessagePair? updateUser(UIAction action)
        {
            var uuAction = (UpdateUserAction)action;
            var userInfo = new UserInfo(uuAction.name, "status", uuAction.imgPath, Guid.Empty);

            return new TypeMessagePair(typeof(UserUpdateMessage), new UserUpdateMessage(userInfo));
        }

        private TypeMessagePair? leaveParty(UIAction action)
        {
            _client.Disconnect();
            return null;
        }


    }
}
