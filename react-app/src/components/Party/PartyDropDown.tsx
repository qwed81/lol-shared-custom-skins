import React from "react";
import '../../css/components/Party/PartyDropDown.scss';
import { IPerson } from "../../Models";

export interface IPartyDropDownProps {
    y: number;
    selectedUser: IPerson | null;
}

export const PartyDropDown = (props: IPartyDropDownProps): JSX.Element => {
    const style: any = {
        top: props.y,
    }

    if(props.selectedUser == null) // only override hover if force close
        style.visibility ='collapse'

    
    return (
    <div className="party-drop-down-component" style={style}>
        <button>Kick User</button>
        <button>Set Admin</button>
        <button>Placeholder</button>
        <button>Placeholder</button>
    </div>);
}