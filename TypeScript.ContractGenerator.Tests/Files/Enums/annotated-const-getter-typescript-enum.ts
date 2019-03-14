
export type AnnotatedEnumWithConstGetterContainingRootType = {
    defaultEnum: DefaultEnum.A;
    explicitEnum: ExplicitEnum.C;
};
export enum DefaultEnum {
    A = 'A',
    B = 'B',
}
export enum ExplicitEnum {
    C = 'C',
    D = 'D',
}
