
export type GlobalNullableRootType = {
    int: number;
    nullableInt?: null | number;
    nullableInts?: null | Nullable<number>[];
    intGeneric?: null | GenericClass<number>;
    nullableIntGeneric?: null | GenericClass<Nullable<number>>;
};
export type Nullable<T> = {
    hasValue: boolean;
    value: T;
};
export type GenericClass<T> = {
    genericType?: T;
};
