// tslint:disable
// TypeScriptContractGenerator's generated content
import { User } from './../DataTypes/User';
import { Guid } from './../DataTypes/Guid';
import { ApiBase } from './../ApiBase/ApiBase';

export class UserApi extends ApiBase implements IUserApi {
    async createUser(user: User): Promise<void> {
        return this.makePostRequest(`/v1/users`, {
            
        }, {
            ...user,
        });
    }

    async deleteUser(userId: Guid): Promise<void> {
        return this.makeDeleteRequest(`/v1/users/${userId}`, {
            
        }, {
            
        });
    }

    async getUser(userId: Guid): Promise<User> {
        return this.makeGetRequest(`/v1/users/${userId}`, {
            
        }, {
            
        });
    }

    async searchUsers(name: string): Promise<User[]> {
        return this.makeGetRequest(`/v1/users`, {
            ['name']: name,
        }, {
            
        });
    }

};
export interface IUserApi {
    createUser(user: User): Promise<void>;
    deleteUser(userId: Guid): Promise<void>;
    getUser(userId: Guid): Promise<User>;
    searchUsers(name: string): Promise<User[]>;
}
