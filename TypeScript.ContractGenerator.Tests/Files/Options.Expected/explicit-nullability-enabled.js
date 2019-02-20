
export type ExplicitNullabilityRootType = {
    someNotNullClass: SomeClass;
    someNullableClass?: null | SomeClass;
    notNullString: string;
    nullableString?: null | string;
    notNullInt: number;
    nullableInt?: null | number;
    notNullArray: number[];
    nullableArray?: null | number[];
    notNullNullablesArray: Array<null | number>;
    nullableNullablesArray?: null | Array<null | number>;
};
export type SomeClass = {
    a: number;
};
