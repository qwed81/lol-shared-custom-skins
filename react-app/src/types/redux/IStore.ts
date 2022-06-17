import { IMod } from "../models/IMod";
import { IUser } from "../models/IUser";

export const enum FilterState {
    Active, Installed, All
}

export interface IStore {
    modFilter: { search: string, filterState: FilterState},
    installedMods: IMod[],
    activeMods: IMod[],
    partyUsers: IUser[],
    userSettings: { user: IUser },
    dropDown: { y: number, open: boolean, user: IUser }

}