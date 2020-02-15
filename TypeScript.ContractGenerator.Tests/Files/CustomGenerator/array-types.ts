
export type ArrayRootType = {
    ints?: null | number[];
    nullableInts?: null | Array<null | number>;
    byteArray?: null | string;
    nullableByteArray?: null | Array<null | number>;
    enums?: null | AnotherEnum[];
    nullableEnums?: null | Array<null | AnotherEnum>;
    strings?: null | Array<null | string>;
    customTypes?: null | Array<null | AnotherCustomType>;
    stringsList?: null | string[];
    intKeyDict?: null | {
        [key: number]: AnotherEnum;
    };
    enumKeyDict?: null | {
        [key in AnotherEnum]?: AnotherEnum;
    };
    customTypesDict?: null | {
        [key: string]: AnotherCustomType;
    };
    set?: null | string[];
    notNullsArray?: null | AnotherCustomType[];
    notNullNotNullsArray: AnotherCustomType[];
};
export enum AnotherEnum {
    B = 'B',
    C = 'C',
}
export type AnotherCustomType = {
    d: number;
};
