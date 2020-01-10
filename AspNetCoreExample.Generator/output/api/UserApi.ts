// tslint:disable
// TypeScriptContractGenerator's generated content
import { User } from './../dto/User';
import { Guid } from './../dataTypes/Guid';
import { ApiBase } from './../apiBase/ApiBase';

export class UserApi extends ApiBase implements IUserApi {
    async createUser(user: User): Promise<void> {
        return this.post(`user/`, {
            
        }, {
            ...user,
        });
    }

    async deleteUser(userId: Guid): Promise<void> {
        return this.delete(`user/${userId}`, {
            
        }, {
            
        });
    }

    async getUser(userId: Guid): Promise<User> {
        return this.get(`user/${userId}`, {
            
        }, {
            
        });
    }

}

export interface IUserApi {
    createUser(user: User): Promise<void>;
    deleteUser(userId: Guid): Promise<void>;
    getUser(userId: Guid): Promise<User>;
}
