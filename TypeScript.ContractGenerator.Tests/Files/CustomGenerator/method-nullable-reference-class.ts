
export interface NullableReferenceMethodType {
    get(s: null | string, nns: string): null | object;
    getNotNull(s: string): object;
    getAsync(s: string): Promise<null | object>;
    postAsync(s: null | string): null | Promise<void>;
    string: string;
    nullableString: null | string;
}
