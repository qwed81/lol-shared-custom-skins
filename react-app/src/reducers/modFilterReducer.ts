type actionType = { type: string, payload: number };
type stateType = { number: number };


export const numberReducer = (state: stateType = {number: 0}, action: actionType): stateType => {
    switch(action.type) {
        case 'INC':
            return {...state, number: state.number + 1};
        default:
            return state;
    }
}