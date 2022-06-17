import React from "react";
import '../css/components/Menu.scss';

export const Menu: React.FC = (): JSX.Element => {
    return (<div className="menu-component">
       
        <h2>Mods</h2>

        <div className="line"></div>

        <div className="button-container">
            <button className="btn selected">Active</button>
            <button className="btn">Installed</button>
            <button className="btn">All</button>
        </div>

        <div className="line"></div>

        <input className="search" placeholder="Search"/>

        <div className="line"></div>

        <div className="commit">
            <p>3 changes</p>
            <button className="btn">Apply</button>
        </div>

        <div className="line"></div>

        <button className="info"></button>

    </div>);
};

const User: React.FC = (): JSX.Element => {
    return (<div className="user-component">

    </div>)
}