
export type NullableReferenceType = {
    string: string;
    nullableString?: null | string;
    array: string[];
    nullableStringArray: Array<null | string>;
    nullableArrayOfStrings?: null | string[];
    nullableArrayOfNullableStrings?: null | Array<null | string>;
    list: string[];
    nullableList?: null | Array<null | string>;
    dictionary: {
        [key in string]?: string;
    };
    dictionaryWithNullableKey: {
        [key in null | string]?: string;
    };
    dictionaryWithNullableValue: {
        [key in string]?: null | string;
    };
    nullableDictionary?: null | {
        [key in null | string]?: null | string;
    };
    valueTypeKeyDictionary: {
        [key in null | number]?: string;
    };
    complexDictionary: {
        [key in string[]]?: string;
    };
    tuple: Tuple<string, string, string>;
    nullableFirstItemTuple: Tuple<string, string, string>;
    nullableSecondItemTuple: Tuple<string, string, string>;
    nullableThirdItemTuple: Tuple<string, string, string>;
    nullableTuple?: null | Tuple<string, string, string>;
    innerTuples?: null | Tuple<string[], string[], string[]>;
};
export type Tuple<T1, T2, T3> = {
    item1: T1;
    item2: T2;
    item3: T3;
};
