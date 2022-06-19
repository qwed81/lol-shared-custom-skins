import { PayloadAction } from "@reduxjs/toolkit"

export const enum FilterState {
    Active = 'Active', Installed = 'Installed', All = 'All'
}

export const enum ModFilterActions {
    SetModFilterSearch = 'SET_MOD_FILTER_SEARCH',
    SetModFilterState = 'SET_MOD_FILTER_STATE'
}

export interface IModFilterState {
    search: string, filterState: FilterState
}

export function SetModFilterSearch(search: string): PayloadAction<string> {
    return { type: ModFilterActions.SetModFilterSearch, payload: search }
}

export function SetModFilterState(state: FilterState): PayloadAction<string> {
    return { type: ModFilterActions.SetModFilterState, payload: state }
}

const initFilterState: IModFilterState = {
    filterState: FilterState.Active,
    search: ''
}

export const modFilterReducer = (state: IModFilterState = initFilterState, action: PayloadAction<IModFilterState>): IModFilterState => {
    switch(action.type) {
        case ModFilterActions.SetModFilterSearch:
            return {...state, }
    }

    return 0 as any;
}