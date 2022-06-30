export const enum PersonStatus {

    Ok = 'ok', Error = 'error', Loading = 'loading', Patching = 'patching'

}

export const enum UserRole {
    Host, Admin, Member
}

export interface IPerson {

    username: string;
    status: PersonStatus;
    imagePath: string;
    uniqueId: string;

}

export interface IMod {
 
    description: string;
    name: string;
    author: string;
    providerName: string;
    imagePath: string;
    fileHash: string;
    downloadingPercentage: number;
    active: boolean;

}

export interface IInvite {
    publicIp: string;
    localIp: string;
    port: number;
    password: string;
}