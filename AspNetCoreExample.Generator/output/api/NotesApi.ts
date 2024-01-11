// tslint:disable
// TypeScriptContractGenerator's generated content
import { Guid } from './../DataTypes/Guid';
import { BlogEntry } from './../DataTypes/BlogEntry';
import { request } from './../ApiBase/ApiBase';
import { url } from './../ApiBase/ApiBase';

export class NotesApi implements INotesApi {
    addEntry(userId: Guid, entry: BlogEntry): Promise<void> {
        return request('POST', url`/v1/user/${userId}/blog`, entry);
    }

    addEntries(userId: Guid, entries: BlogEntry[]): Promise<void> {
        return request('POST', url`/v1/user/${userId}/blog/batch`, entries);
    }

};
export interface INotesApi {
    addEntry(userId: Guid, entry: BlogEntry): Promise<void>;
    addEntries(userId: Guid, entries: BlogEntry[]): Promise<void>;
}
