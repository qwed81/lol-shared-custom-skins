import React, { useContext, useRef, useState } from "react";
import { PartyMember } from "./PartyMember";
import '../../css/components/Party/Party.scss';
import { User } from "./User";
import { PartyDropDown } from "./PartyDropDown";
import { IPerson } from "../../Models";
import { ClientContext, IClientContext } from "../ClientProvider/ClientContextProvider";

interface IUserHovered {
    userElementTop: number,
    selectedUserId: string | null
}

export const Party: React.FC = (): JSX.Element => {
    const listRef = useRef<HTMLDivElement>(null);
    const init = {userElementTop: 0, selectedUserId: null}
    let [selection, setSelection] = useState<IUserHovered>(init);
    let clientContext: IClientContext = useContext<IClientContext>(ClientContext);

    function userMouseEnter(userElementTop: number, userId: string): void {
        let minTop: number | undefined = listRef.current?.getBoundingClientRect().top;
        if(minTop && userElementTop < minTop)
            userElementTop = minTop

        setSelection({userElementTop, selectedUserId: userId});
    }

    function deselectAll(): void {
        setSelection({...init, selectedUserId: null});
    }

    function getSelectedUser(): IPerson | null {
        if(selection.selectedUserId == null)
            return null;
        
            return clientContext.partyMembers.filter(user => user.uniqueId == selection.selectedUserId)[0];
    }

    return (
    <div className="party-component">
        <div className="title-container">
            <h2>Party</h2>
            <button className="create-invite">Create Invite</button>
        </div>
        
        <div className="party-member-list" onScroll={deselectAll} onMouseLeave={deselectAll} ref={listRef}>
            {clientContext.partyMembers.map(member =>
                <PartyMember key={member.uniqueId} user={member} 
                 selected={selection.selectedUserId == member.uniqueId}
                 userMouseEnter={userMouseEnter}/>)
                }
            <PartyDropDown y={selection.userElementTop} selectedUser={getSelectedUser()}/>
        </div>

        <User />
    </div>);
};