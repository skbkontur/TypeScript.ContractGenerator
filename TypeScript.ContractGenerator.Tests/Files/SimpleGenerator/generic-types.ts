
export type GenericContainingRootType = {
    genericIntChild?: null | GenericChildType<number>;
    genericNullableIntChild?: null | GenericChildType<number>;
    arrayGenericNullableIntChild?: null | GenericChildType<number>[];
    severalGenericParameters?: null | ChildWithSeveralGenericParameters<string, number>;
    genericChildType?: null | GenericChildType<ChildWithConstraint<string>>;
    genericHell?: null | GenericChildType<ChildWithConstraint<GenericChildType<string>>>;
    genericNotNullAttribute?: null | GenericChildTypeWithAttributes<number>;
};
export type GenericChildType<T> = {
    childType?: null | T;
    childTypes?: null | T[];
};
export type ChildWithSeveralGenericParameters<T1, T2> = {
    item1?: null | T1;
    item2?: null | T2;
};
export type ChildWithConstraint<T> = {
    child?: null | T;
};
export type GenericChildTypeWithAttributes<T> = {
    notNullProperty: T;
};
