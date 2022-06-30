import React, { MouseEventHandler, useRef } from "react";
import '../../css/components/Party/PartyMember.scss';
import { IPerson } from "../../Models";

export interface IPartyMemberProps {
    user: IPerson;
    selected: boolean;
    userMouseEnter: (userElementTop: number, userId: string) => void;
}

export const PartyMember = (props: IPartyMemberProps): JSX.Element => {
    const elemRef = useRef<HTMLDivElement>(null)

    function handleMouseEnter() {
        let rect = elemRef.current?.getBoundingClientRect();
        props.userMouseEnter(rect!.top, props.user.uniqueId);
    }

    return (
        <div className={`party-member-component person ${props.selected ? 'selected' : ''}`} ref={elemRef}
            onMouseEnter={handleMouseEnter}>

            <div className="pfp" style={{
                backgroundImage: `url(${props.user.imagePath})`,
            }}>
            </div>

            <div className="text">
                <p className="username">
                    {props.user.username}
                </p>

                <p className="status-text">
                    status:&nbsp;
                    <span className={'status ' + props.user.status}>
                        {props.user.status}
                    </span>
                </p>
            </div>
        </div>
    );
}