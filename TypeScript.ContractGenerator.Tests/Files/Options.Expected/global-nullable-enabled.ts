
export type GlobalNullableRootType = {
    int: number;
    nullableInt?: Nullable<number>;
    nullableInts?: Nullable<Nullable<number>[]>;
    intGeneric?: Nullable<GenericClass<number>>;
    nullableIntGeneric?: Nullable<GenericClass<Nullable<number>>>;
};
export type GenericClass<T> = {
    genericType?: T;
};
