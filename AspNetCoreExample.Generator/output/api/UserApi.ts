// tslint:disable
// TypeScriptContractGenerator's generated content
import { User } from './../DataTypes/User';
import { Guid } from './../DataTypes/Guid';
import { ApiBase } from './../ApiBase/ApiBase';
import { url } from './../ApiBase/ApiBase';

export class UserApi extends ApiBase implements IUserApi {
    createUser(user: User): Promise<void> {
        return this.makePostRequest(url`/v1/users`, user);
    }

    deleteUser(userId: Guid): Promise<void> {
        return this.makeDeleteRequest(url`/v1/users/${userId}`);
    }

    getUser(userId: Guid): Promise<User> {
        return this.makeGetRequest(url`/v1/users/${userId}`);
    }

    searchUsers(name: string): Promise<User[]> {
        return this.makeGetRequest(url`/v1/users?name=${name}`);
    }

};
export interface IUserApi {
    createUser(user: User): Promise<void>;
    deleteUser(userId: Guid): Promise<void>;
    getUser(userId: Guid): Promise<User>;
    searchUsers(name: string): Promise<User[]>;
}
