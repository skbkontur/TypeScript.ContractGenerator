
export type ArrayRootType = {
    ints?: null | number[];
    nullableInts?: null | Array<null | number>;
    byteArray?: null | string;
    nullableByteArray?: null | Array<null | number>;
    enums?: null | AnotherEnum[];
    nullableEnums?: null | Array<null | AnotherEnum>;
    strings?: null | Array<null | string>;
    customTypes?: null | Array<null | AnotherCustomType>;
    stringsList?: null | Array<null | string>;
    customTypesDict?: null | {
        [key: string]: AnotherCustomType;
    };
    set?: null | HashSet<string>;
    notNullsArray?: null | AnotherCustomType[];
};
export type AnotherEnum = 'B' | 'C';
export const AnotherEnums = {
    ['B']: (('B'): AnotherEnum),
    ['C']: (('C'): AnotherEnum),
};
export type AnotherCustomType = {
    d: number;
};
export type HashSet<T> = {
    count: number;
    comparer: IEqualityComparer<T>;
};
export type IEqualityComparer<T> = {
};
