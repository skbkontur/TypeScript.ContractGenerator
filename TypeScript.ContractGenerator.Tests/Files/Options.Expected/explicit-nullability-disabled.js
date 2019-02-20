
export type ExplicitNullabilityRootType = {
    someNotNullClass: SomeClass;
    someNullableClass?: SomeClass;
    notNullString: string;
    nullableString?: string;
    notNullInt: number;
    nullableInt?: number;
    notNullArray: number[];
    nullableArray?: number[];
    notNullNullablesArray: number[];
    nullableNullablesArray?: number[];
};
export type SomeClass = {
    a: number;
};
