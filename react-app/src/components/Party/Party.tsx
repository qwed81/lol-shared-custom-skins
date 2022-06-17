import React from "react";
import { PartyMember } from "./PartyMember";
import '../../css/components/Party/Party.scss';
import { User } from "./User";

export const Party: React.FC = (): JSX.Element => {
    return (<div className="party-component">
        <div className="title-container">
            <h2>Party</h2>
            <button className="create-invite">Create Invite</button>
        </div>
        
        <div className="party-member-list">

            <PartyMember/>
            <PartyMember/>
            <PartyMember/>
            <PartyMember/>
            <PartyMember/>
        
        </div>

        <User />
    </div>);
};