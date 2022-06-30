import { IInvite, IMod, IPerson } from "../../Models";

// actions
export const enum ActionType {
    Apply, ChangeActivation, Join, CreateInvite, UpdateUser
}

export interface ApplyAction {
    type: ActionType.Apply;
}

export interface ChangeActivationAction {
    type: ActionType.ChangeActivation;
    modId: string;
    value: boolean;
}

export interface JoinAction {
    type: ActionType.Join;
    invite: IInvite;
}

export interface CreateInviteAction {
    type: ActionType.CreateInvite;
    count: number;
}

export interface UpdateUserAction {
    type: ActionType.UpdateUser;
    name: string;
    imgPath: string;
}


// data update
export const enum DataUpdateType {
    ChangesCount, ModList, SingleMod, PartyMembers, User, OpenInvite, PartyConnected    
}

export interface ChangesCountDataUpdate {
    type: DataUpdateType.ChangesCount;
    count: number;
}

export interface ModListDataUpdate {
    type: DataUpdateType.ModList;
    mods: IMod[];
}

export interface SingleModDataUpdate {
    type: DataUpdateType.SingleMod;
    mod: IMod;
}

export interface PartyMembersDataUpdate {
    type: DataUpdateType.PartyMembers;
    members: IPerson[];
}

export interface UserDataUpdate {
    type: DataUpdateType.User;
    user: IPerson;
}

export interface OpenInviteDataUpdate {
    type: DataUpdateType.OpenInvite;
    invite: IInvite;
}

export type Action = ApplyAction | ChangeActivationAction | JoinAction | CreateInviteAction;
export type DataUpdate = ChangesCountDataUpdate | ModListDataUpdate | SingleModDataUpdate | PartyMembersDataUpdate | UserDataUpdate | OpenInviteDataUpdate;




