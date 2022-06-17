export const enum Status {

    Ok = 'Ok', Error = 'Error', Loading = 'Loading', Patching = 'Patching'

}

export interface IUser {

    username: string;
    status: Status;
    imagePath: string;
    uniqueId: string;

}