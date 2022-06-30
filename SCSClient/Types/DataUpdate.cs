namespace SCSClient.Types
{

    public enum DataUpdateType
    {
        ChangesCount, ModList, SingleMod, PartyMembers, User, OpenInvite,
        PartyConnectected, PartyDisconnected, PartyLoading
    }

    public record class DataUpdate(DataUpdateType type);

    public record class ChangesCountDataUpdate(int count) : DataUpdate(DataUpdateType.ChangesCount);

    public record class ModListDataUpdate(List<ReactMod> mods) : DataUpdate(DataUpdateType.ModList);

    public record class SingleModDataUpdate(ReactMod mod) : DataUpdate(DataUpdateType.SingleMod);

    public record class PartyMembersDataUpdate(List<ReactPerson> members) : DataUpdate(DataUpdateType.PartyMembers);

    public record class UserDataUpdate(ReactPerson user) : DataUpdate(DataUpdateType.User);

    public record class PartyConnectedDataUpdate() : DataUpdate(DataUpdateType.SingleMod);

    public record class PartyDisconnectedDataUpdate() : DataUpdate(DataUpdateType.SingleMod);

    public record class PartyLoadingDataUpdate() : DataUpdate(DataUpdateType.SingleMod);

    /*

    public record class OpenInvite(string fileHash) : DataResponse(DataResponseType.SingleMod);

    */

}
