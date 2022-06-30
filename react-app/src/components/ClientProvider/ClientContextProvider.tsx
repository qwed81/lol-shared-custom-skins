import React, { Component, PropsWithChildren, ReactNode } from "react";
import { IInvite, IMod, IPerson } from "../../Models";
import { Action, ActionType, ChangesCountDataUpdate, DataUpdate, DataUpdateType, ModListDataUpdate, OpenInviteDataUpdate, SingleModDataUpdate, PartyMembersDataUpdate, UserDataUpdate } from "./Types";

interface IClientState {
    mods: IMod[];
    partyMembers: IPerson[];
    user: IPerson | null;
    changesCount: number;
    inviteOpen: boolean;
    createdInvite: IInvite | null;
}

export interface IClientContext extends IClientState {
    apply: () => void;
    changeActivation: (modId: string, value: boolean) => void;
    join: (invite: IInvite) => void;
    createInvite: (count: number) => void;
}  

const initContext: IClientContext = {
    mods: [],
    partyMembers: [],
    user: null,
    changesCount: 0,
    inviteOpen: false,
    createdInvite: null,
    apply: () => {},
    changeActivation: () => {},
    join: () => {},
    createInvite: () => {}
}

export const ClientContext = React.createContext<IClientContext>(initContext);

export class ClientContextProvider extends Component<PropsWithChildren, IClientState> {

    private socket: WebSocket;

    public constructor(props: {}) {
        super(props);

        this.state = {
            mods: [],
            partyMembers: [],
            user: null,
            changesCount: 0,
            inviteOpen: false,
            createdInvite: null,
        }

        this.socket = null!; // will be set on mount
    }

    public componentDidMount() {
        this.socket = new WebSocket('ws://localhost:5000');
        this.socket.onmessage = this.handleMessage;
    }

    public componentWillUnmount() {
        this.socket!.onmessage = null;
        this.socket.close();
    }

    private handleMessage = (e: MessageEvent<string>) => {
        let msg = JSON.parse(e.data) as DataUpdate;
        console.log('message got', msg);
        switch(msg.type) {
            case DataUpdateType.ModList: 
                this.setState(prevState => {
                    return {...prevState, mods: (msg as ModListDataUpdate).mods}
                });
                break;
            case DataUpdateType.PartyMembers:
                this.setState(prevState => {
                    return {...prevState, partyMembers: (msg as PartyMembersDataUpdate).members}
                });
                break;
            case DataUpdateType.User: 
                this.setState(prevState => {
                    return {...prevState, user: (msg as UserDataUpdate).user}
                });
                break;
            case DataUpdateType.ChangesCount:
                this.setState(prevState => {
                    return {...prevState, changesCount: (msg as ChangesCountDataUpdate).count}
                });
                break;
            case DataUpdateType.SingleMod:
                this.setState(prevState => {
                    let newMod = (msg as SingleModDataUpdate).mod;
                    let newList = prevState.mods.map(mod => mod.fileHash == newMod.fileHash ? newMod : mod);
                    return {...prevState, mods: newList}
                });
                break;
            case DataUpdateType.OpenInvite:
                this.setState(prevState => {
                    return {...prevState, createdInvite: (msg as OpenInviteDataUpdate).invite, inviteOpen: true}
                })
                break;            
        }
    }

    private sendAction(action: Action): void {
        console.log('sending ', action);
        this.socket?.send(JSON.stringify(action));
    }

    public render(): ReactNode {
        return (
            <ClientContext.Provider value={{
                mods: this.state.mods,
                partyMembers: this.state.partyMembers,
                user: this.state.user,
                changesCount: this.state.changesCount,
                inviteOpen: this.state.inviteOpen,
                createdInvite: this.state.createdInvite,
                apply: () => this.sendAction({type: ActionType.Apply}),
                changeActivation: (modId: string, value: boolean) => this.sendAction({type: ActionType.ChangeActivation, modId, value}),
                join: (invite: IInvite) => this.sendAction({type: ActionType.Join, invite}),
                createInvite: (count: number) => this.sendAction({type: ActionType.CreateInvite, count})
            }}>
                {this.props.children}
            </ClientContext.Provider>
        );
    }
}