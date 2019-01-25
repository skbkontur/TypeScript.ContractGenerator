
export type NotNullRootType = {
    someNotNullClass: SomeClass;
    someNullableClass?: null | SomeClass;
    notNullString: string;
};
export type SomeClass = {
    a: number;
};
