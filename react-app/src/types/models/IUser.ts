export const enum UserStatus {

    Ok = 'ok', Error = 'error', Loading = 'loading', Patching = 'patching'

}

export const enum UserRole {
    Host, Admin, Member
}

export interface IUser {

    username: string;
    status: UserStatus;
    imagePath: string;
    uniqueId: string;

}