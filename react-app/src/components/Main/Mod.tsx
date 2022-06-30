import React, { useContext, useState } from "react";
import '../../css/components/Main/Mod.scss'
import { IMod } from "../../Models";
import { ClientContext, IClientContext } from "../ClientProvider/ClientContextProvider";

export interface IModProps {
    mod: IMod;
    modClicked: (mod: IMod) => void;
}

export const Mod = ({mod, modClicked}: IModProps): JSX.Element => {
    let clientContext: IClientContext = useContext<IClientContext>(ClientContext);
    let [localActive, setLocalActive] = useState(mod.active);

    function changeActivation(value: boolean) {
        setLocalActive(value);
        setTimeout(() => {clientContext.changeActivation(mod.fileHash, value)}, 300);
    }

    return (
        <div className="mod-component">
            <div className="picture-container">
                {mod.downloadingPercentage >= 100 ? 
                    <div className="mod-picture" style={{
                        backgroundImage: `url(${mod.imagePath})`
                    }} onClick={() => modClicked(mod)}>
                    </div>
                    :
                    <div className="mod-loading">
                        <h1>Loading</h1>
                        <div className="progress-bar-container">
                            <div className="progress-bar" style={{
                                width: mod.downloadingPercentage + '%'
                            }}>
                            </div>
                        </div>
                        <h2>{mod.downloadingPercentage}%</h2>
                    </div>

                }
                
            </div>

            <div className="control">
                <h4>{mod.name}</h4>        
                
                <div className="active-container">
                    <label className="switch">
                        <input type="checkbox" tabIndex={-1} checked={localActive}
                            onChange={(e) => changeActivation(e.target.checked)} 
                            disabled={mod.downloadingPercentage < 100}/>
                        <span className="slider"/>
                    </label>
                </div>
            </div>
        </div>
    );
}
