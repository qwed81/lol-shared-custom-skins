using Models.Network;

namespace SCSClient
{
    
    public enum UIActionType
    {
        ApplyChanges, ChangeActivation, JoinParty, CreateInvite, UpdateUser, LeaveParty
    }

    public record class UIAction(UIActionType type);

    public record class ApplyAction() : UIAction(UIActionType.ApplyChanges);

    public record class ChangeActivationAction(string fileHash, bool value) : UIAction(UIActionType.ChangeActivation);

    public record class JoinPartyAction(InviteInfo invite) : UIAction(UIActionType.JoinParty);

    public record class CreateInviteAction() : UIAction(UIActionType.CreateInvite);

    public record class UpdateUserAction(string name, string imgPath) : UIAction(UIActionType.UpdateUser);

    public record class leavePartyAction() : UIAction(UIActionType.LeaveParty);


}
