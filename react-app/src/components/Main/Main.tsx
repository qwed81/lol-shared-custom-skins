import React, { useRef, useState } from "react";
import '../../css/components/Main/Main.scss';
import { IMod } from "../../types/models/IMod";
import { mods } from "../../test-data/mods";
import { Mod } from "./Mod";
import { ModPopup } from "./ModPopup";

export const Main: React.FC = (): JSX.Element => {
    const emptyMod: IMod = {author: '', description: '', downloadingPercentage: 0, imagePath: '', uniqueId: '', name: '', providerName: ''};
    let [open, setOpen] = useState<boolean>(false);
    let [selectedMod, setSelectedMod] = useState<IMod>(emptyMod);
    
    return (
    <div className="main-component">
        <div className="mod-list">
            {mods.map(mod => 
            <div key={mod.uniqueId} className="mod-container">
                <Mod mod={mod} modClicked={() => {setOpen(true); setSelectedMod(mod)}}/>
            </div>
            )}
            <PlaceHolder/>
            <ModPopup mod={selectedMod} open={open} closePopup={() => setOpen(false)}/>
        </div>
    </div>);
};

const PlaceHolder = (): JSX.Element => { // hack for flexbox (fill in extra places so all mods remain same size)
    return (
    <>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
            <div className="placeholder"></div>
    </>);
}
