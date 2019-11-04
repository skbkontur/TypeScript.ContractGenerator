
export type NullableReferenceType = {
    string: string;
    nullableString: string;
    array: string[];
    nullableStringArray: string[];
    nullableArrayOfStrings: string[];
    nullableArrayOfNullableStrings: string[];
    list: string[];
    nullableList: string[];
    dictionary: {
        [key in string]?: string;
    };
    dictionaryWithNullableKey: {
        [key in string]?: string;
    };
    dictionaryWithNullableValue: {
        [key in string]?: string;
    };
    nullableDictionary: {
        [key in string]?: string;
    };
    tuple: Tuple<string, string, string>;
    nullableFirstItemTuple: Tuple<string, string, string>;
    nullableSecondItemTuple: Tuple<string, string, string>;
    nullableThirdItemTuple: Tuple<string, string, string>;
    nullableTuple: Tuple<string, string, string>;
};
export type Tuple<T1, T2, T3> = {
    item1: T1;
    item2: T2;
    item3: T3;
};
