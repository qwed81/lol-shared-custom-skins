import React, { PropsWithChildren, useState } from 'react';

export const enum FilterCatagory {
    Active, Installed, All
}

export interface IFilterContext {
    search: string;
    setSearch: React.Dispatch<React.SetStateAction<string>>;
    catagory: FilterCatagory;
    setCatagory: (catagory: FilterCatagory) => void;
}

export let FilterContext = React.createContext<IFilterContext>({search: '', setSearch: () => {}, catagory: FilterCatagory.Active, setCatagory: () => {}});

export const FilterContextProvider = ({children}: PropsWithChildren) => {
    let [search, setSearch] = useState<string>('');
    let [catagory, setCatagory] = useState<FilterCatagory>(FilterCatagory.Active);

    return (
        <FilterContext.Provider value={{search, setSearch, catagory, setCatagory}}>
            {children}
        </FilterContext.Provider>
    )

}