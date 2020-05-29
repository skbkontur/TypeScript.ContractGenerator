
export interface NullableReferenceMethodType {
    get(s: string, nns: null | string): object;
    getNotNull(s: null | string): null | object;
    getAsync(s: null | string): null | Promise<null | object>;
    postAsync(s: string): Promise<void>;
    string: null | string;
    nullableString: string;
}
