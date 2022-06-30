import React, { useContext } from "react";
import '../css/components/Menu.scss';
import { FilterCatagory, FilterContext, IFilterContext } from "./FilterContext";

export const Menu: React.FC = (): JSX.Element => {
    let filterContext: IFilterContext = useContext(FilterContext);
    
    return (<div className="menu-component">
       
        <h2>Mods</h2>

        <div className="line"></div>

        <div className="button-container">
            <button className={`btn ${filterContext.catagory == FilterCatagory.Active ? 'selected' : ''}`}  
                onClick={() => filterContext.setCatagory(FilterCatagory.Active)}>
                Active
            </button>
            
            <button className={`btn ${filterContext.catagory == FilterCatagory.Installed ? 'selected' : ''}`} 
                onClick={() => filterContext.setCatagory(FilterCatagory.Installed)}>
                Inactive
            </button>
            <button className={`btn ${filterContext.catagory == FilterCatagory.All ? 'selected' : ''}`}
                onClick={() => filterContext.setCatagory(FilterCatagory.All)}>
                All
            </button>
        </div>

        <div className="line"></div>

        <input className="search" placeholder="Search" 
            value={filterContext.search} onChange={e => filterContext.setSearch(e.target.value)}/>

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