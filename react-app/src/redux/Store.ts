import { IMod } from "../types/models/IMod";
import { IUser } from "../types/models/IUser";
import { configureStore } from "@reduxjs/toolkit";
import { modFilterReducer } from "./reducers/modFilterReducer";

export interface IStore {
    installedMods: IMod[],
    activeMods: IMod[],
    partyUsers: IUser[],
    userSettings: { user: IUser },
    dropDown: { y: number, open: boolean, user: IUser }

}

const reducer = {
    modFilter: modFilterReducer,
    
}

const configureStoreOptions = {
    reducer,
    devTools: true
}

