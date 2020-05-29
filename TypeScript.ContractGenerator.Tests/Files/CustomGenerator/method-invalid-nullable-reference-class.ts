
export interface MethodRootType {
    get(s: string, nns: string): object;
    getNotNull(s: string): object;
    getAsync(s: string): Promise<object>;
    postAsync(s: string): Promise<void>;
    string: string;
    nullableString: string;
}
