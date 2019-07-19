import { ByteArray } from './DataTypes/ByteArray';
import { AnotherCustomType } from './a/b/c';

export type ArrayRootType = {
    ints?: null | number[];
    nullableInts?: null | Array<null | number>;
    byteArray?: null | ByteArray;
    nullableByteArray?: null | Array<null | number>;
    enums?: null | AnotherEnum[];
    nullableEnums?: null | Array<null | AnotherEnum>;
    strings?: null | string[];
    customTypes?: null | AnotherCustomType[];
    stringsList?: null | string[];
    customTypesDict?: null | {
        [key in string]?: AnotherCustomType;
    };
    set?: null | string[];
};
export type AnotherEnum = 'B' | 'C';
export const AnotherEnums = {
    ['B']: ('B') as AnotherEnum,
    ['C']: ('C') as AnotherEnum,
};
