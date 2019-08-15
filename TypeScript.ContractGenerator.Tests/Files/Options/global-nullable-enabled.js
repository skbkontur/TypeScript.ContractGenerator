
export type GlobalNullableRootType = {
    int: number;
    nullableInt?: ?number;
    nullableInts?: ??number[];
    intGeneric?: ?GenericClass<number>;
    nullableIntGeneric?: ?GenericClass<number>;
};
export type GenericClass<T> = {
    genericType?: ?T;
};
