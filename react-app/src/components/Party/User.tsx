import React from "react";
import '../../css/components/Party/User.scss';

const bgiStr = 'url("https://bandlabimages.azureedge.net/v1.0/users/11f301f4-ea39-4ed2-9898-caad91981ab7/640x640")';

export const User: React.FC = (): JSX.Element => {
    return ( <div className="user-component">
        <div className="pfp" style={
            {backgroundImage: bgiStr,
        }}>
        </div>

        <div className="text">
            <p className="username">
                Username
            </p>

            <p className="status">
                LOADING
            </p>
        </div>

        <button className="action-btn leave-party"></button>
        <button className="action-btn settings"></button>
    </div>);

}
