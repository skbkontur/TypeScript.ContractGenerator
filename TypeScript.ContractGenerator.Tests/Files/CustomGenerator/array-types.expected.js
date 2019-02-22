
export type ArrayRootType = {
    ints?: null | number[];
    nullableInts?: null | Array<null | number>;
    byteArray?: null | string;
    nullableByteArray?: null | Array<null | number>;
    enums?: null | AnotherEnum[];
    nullableEnums?: null | Array<null | AnotherEnum>;
    strings?: null | string[];
    customTypes?: null | AnotherCustomType[];
    stringsList?: null | string[];
    customTypesDict?: null | {
        [key: string]: AnotherCustomType;
    };
    set?: null | string[];
};
export type AnotherEnum = 'B' | 'C';
export const AnotherEnums = {
    ['B']: (('B'): AnotherEnum),
    ['C']: (('C'): AnotherEnum),
};
export type AnotherCustomType = {
    d: number;
};
