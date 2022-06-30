import React, { useContext } from "react";
import '../../css/components/Party/User.scss';
import { ClientContext, IClientContext } from "../ClientProvider/ClientContextProvider";

export const User: React.FC = (): JSX.Element => {
    let clientContext: IClientContext = useContext<IClientContext>(ClientContext);
    
    return ( <div className="user-component person">
        <div className="pfp" style={
            {backgroundImage: clientContext.user?.imagePath,
        }}>
        </div>

        <div className="text">
            <p className="username">
                {clientContext.user?.username}
            </p>

            <p className="status-text">
                status:&nbsp;
                <span className={'status ' + clientContext.user?.status}>
                    {clientContext.user?.status}
                </span>
            </p>
        </div>

        <button className="action-btn leave-party"></button>
        <button className="action-btn settings"></button>
    </div>);

}
