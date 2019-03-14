
export type ComplexRootType = {
    childStruct: ChildStruct;
    nullableChildStruct?: null | ChildStruct;
    childClass?: null | ChildClass;
};
export type ChildStruct = {
    a: number;
    b?: null | string;
};
export type ChildClass = {
    c: number;
    recursiveChildClass?: null | RecursiveChildClass;
};
export type RecursiveChildClass = {
    childClass?: null | ChildClass;
    rChildClass?: null | RecursiveChildClass;
};
