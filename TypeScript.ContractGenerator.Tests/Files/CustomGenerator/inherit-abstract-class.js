
export type FirstInheritor = {
    a: number;
};
export type SecondInheritor = {
    a?: null | string;
};
export type AbstractClass = FirstInheritor | SecondInheritor;
