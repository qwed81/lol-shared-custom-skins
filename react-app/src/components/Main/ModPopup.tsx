import React from "react";
import ReactDom from "react-dom";
import '../../css/components/Main/ModPopup.scss'
import { IMod } from "../../Models";

export interface IModPopupProps {
    mod: IMod;
    open: boolean;
    closePopup: () => void;
}

export const ModPopup = ({mod, open, closePopup}: IModPopupProps): JSX.Element | null => {
    if(!open)
        return null;

    return ReactDom.createPortal(
    <>
        <div className="mod-popup-background" onClick={closePopup}></div>
        <article className="mod-popup-component">
            
            <img className="mod-banner" src={mod.imagePath}/>
            
            <hr className="divider"/>

            <div className="text">
                <h1>{mod.name}</h1>
                <h2>Author: {mod.author}</h2>
                <h2>Provider: {mod.providerName}</h2>
                <h2>MD5 Hash: {mod.fileHash}</h2>
                <br/>
                <p>{mod.description}</p>
            </div>

        </article>
    </>,
    document.getElementById('portal')!);
}