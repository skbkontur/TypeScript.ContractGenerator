// tslint:disable
// TypeScriptContractGenerator's generated content
import { Guid } from './../DataTypes/Guid';
import { BlogEntry } from './../DataTypes/BlogEntry';
import { ApiBase } from './../ApiBase/ApiBase';

export class NotesApi extends ApiBase implements INotesApi {
    async addEntry(userId: Guid, entry: BlogEntry): Promise<void> {
        return this.makePostRequest(`/v1/user/${userId}/blog`, {
            
        }, {
            ...entry,
        });
    }

    async addEntries(userId: Guid, entries: BlogEntry[]): Promise<void> {
        return this.makePostRequest(`/v1/user/${userId}/blog/batch`, {
            
        }, entries);
    }

};
export interface INotesApi {
    addEntry(userId: Guid, entry: BlogEntry): Promise<void>;
    addEntries(userId: Guid, entries: BlogEntry[]): Promise<void>;
}
