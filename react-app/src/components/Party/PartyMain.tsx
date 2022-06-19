import React, { useRef, useState } from "react";
import { PartyMember } from "./PartyMember";
import '../../css/components/Party/Party.scss';
import { User } from "./User";
import { PartyDropDown } from "./PartyDropDown";
import { IUser, UserStatus } from "../../types/models/IUser";
import { users } from '../../test-data/users'

interface IUserHovered {
    userElementTop: number,
    selectedUserId: string | null
}

export const Party: React.FC = (): JSX.Element => {
    const listRef = useRef<HTMLDivElement>(null);
    const init = {userElementTop: 0, selectedUserId: null}
    let [selection, setSelection] = useState<IUserHovered>(init);

    function userMouseEnter(userElementTop: number, userId: string): void {
        let minTop: number | undefined = listRef.current?.getBoundingClientRect().top;
        if(minTop && userElementTop < minTop)
            userElementTop = minTop

        setSelection({userElementTop, selectedUserId: userId});
    }

    function deselectAll(): void {
        setSelection({...init, selectedUserId: null});
    }

    function getSelectedUser(): IUser | null {
        if(selection.selectedUserId == null)
            return null;
        
            return users.filter(user => user.uniqueId == selection.selectedUserId)[0];
    }

    return (
    <div className="party-component">
        <div className="title-container">
            <h2>Party</h2>
            <button className="create-invite">Create Invite</button>
        </div>
        
        <div className="party-member-list" onScroll={deselectAll} onMouseLeave={deselectAll} ref={listRef}>
            {users.map(user =>
                <PartyMember key={user.uniqueId} user={user} 
                 selected={selection.selectedUserId == user.uniqueId}
                 userMouseEnter={userMouseEnter}/>)
                }
            <PartyDropDown y={selection.userElementTop} selectedUser={getSelectedUser()}/>
        </div>

        <User />
    </div>);
};