import React from "react";
import '../../css/components/Main/Mod.scss'
import { IMod } from "../../types/models/IMod";
import { ModPopup } from "./ModPopup";

export interface IModProps {
    mod: IMod;
    modClicked: (mod: IMod) => void;
}

export const Mod = ({mod, modClicked}: IModProps): JSX.Element => {
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
                <h4>Mod Name</h4>        
                
                <div className="active-container">
                    <label className="switch">
                        <input type="checkbox" tabIndex={-1} disabled={mod.downloadingPercentage < 100}/>
                        <span className="slider"/>
                    </label>
                </div>
            </div>
        </div>
    );
}