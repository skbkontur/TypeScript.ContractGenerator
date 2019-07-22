import { ApiBase } from "./ApiBase";

export class UserApiBase extends ApiBase {
    constructor(prefix: string, userId: string) {
        super(`${prefix}/user/${userId}/`);
    }
}
