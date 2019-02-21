
export type ArrayRootType = {
    ints?: null | number[];
    nullableInts?: null | Array<null | number>;
    byteArray?: null | string;
    nullableByteArray?: null | Array<null | number>;
    enums?: null | AnotherEnum[];
    nullableEnums?: null | Array<null | AnotherEnum>;
    strings?: null | string[];
    customTypes?: null | AnotherCustomType[];
    stringsList?: null | List<string>;
    customTypesDict?: null | Dictionary<string, AnotherCustomType>;
};
export type AnotherEnum = 'B' | 'C';
export const AnotherEnums = {
    ['B']: (('B'): AnotherEnum),
    ['C']: (('C'): AnotherEnum),
};
export type AnotherCustomType = {
    d: number;
};
export type List<T> = {
    capacity: number;
    count: number;
    item?: T;
};
export type Dictionary<TKey, TValue> = {
    comparer: IEqualityComparer<TKey>;
    count: number;
    keys?: null | KeyCollection<TKey, TValue>;
    values?: null | ValueCollection<TKey, TValue>;
    item?: TValue;
};
export type IEqualityComparer<T> = {
};
export type KeyCollection<TKey, TValue> = {
    count: number;
};
export type ValueCollection<TKey, TValue> = {
    count: number;
};
