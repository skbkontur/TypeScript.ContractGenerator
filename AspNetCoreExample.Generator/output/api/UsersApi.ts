// tslint:disable
// TypeScriptContractGenerator's generated content
import { User } from './../dto/User';
import { ApiBase } from './../apiBase/ApiBase';

export class UsersApi extends ApiBase implements IUsersApi {
    async createUsers(users: User[]): Promise<void> {
        return this.post(`users/`, {
            
        }, users);
    }

    async searchUsers(name: string): Promise<User[]> {
        return this.get(`users/`, {
            ['name']: name,
        }, {
            
        });
    }

}

export interface IUsersApi {
    createUsers(users: User[]): Promise<void>;
    searchUsers(name: string): Promise<User[]>;
}
