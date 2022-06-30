import React, { useContext, useRef, useState } from "react";
import '../../css/components/Main/Main.scss';
import { IMod } from "../../Models";
import { Mod } from "./Mod";
import { ModPopup } from "./ModPopup";
import { FilterCatagory, FilterContext, IFilterContext } from "../FilterContext";
import { ClientContext, IClientContext } from "../ClientProvider/ClientContextProvider";

export const Main: React.FC = (): JSX.Element => {
    const emptyMod: IMod = {author: '', description: '', downloadingPercentage: 0, imagePath: '', fileHash: '', name: '', providerName: '', active: true};
    let [open, setOpen] = useState<boolean>(false);
    let [selectedMod, setSelectedMod] = useState<IMod>(emptyMod);
    let filterContext: IFilterContext = useContext(FilterContext);
    let clientContext: IClientContext = useContext(ClientContext);

    function getFilteredMods(): IMod[] {
        const shouldBeActive: boolean = filterContext.catagory == FilterCatagory.Active; 
        return clientContext.mods.filter(mod => filterContext.catagory == FilterCatagory.All 
            || mod.active == shouldBeActive).filter(mod => mod.name.toLowerCase()
            .includes(filterContext.search.toLowerCase()));
    }

    let filteredMods = getFilteredMods();

    return (
    <div className="main-component">
        {filteredMods.length != 0 && 
        <div className="mod-list">
            {filteredMods.map(mod => 
                <div key={mod.fileHash} className="mod-container">            
                    <div className="height-setter"></div>
                    <div className="mod-wrapper">
                        <Mod mod={mod} modClicked={() => {setOpen(true); setSelectedMod(mod)}}/>
                    </div>
                </div>
            )}
            <PlaceHolder/>
        </div>}

        {filteredMods.length == 0 &&
            <div className="no-mod-text">
                <div>
                    <h2>Looks like there are no mods</h2>
                    <h3>Try changing the filter or uploading more</h3>
                </div>
            </div>
        }

        <ModPopup mod={selectedMod} open={open} closePopup={() => setOpen(false)}/>
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
