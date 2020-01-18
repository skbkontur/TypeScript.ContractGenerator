
export type EnumContainingRootType = {
    defaultEnum: DefaultEnum;
    nullableEnum?: null | DefaultEnum;
    explicitEnum: ExplicitEnum;
};
export enum DefaultEnum {
    A = 'A',
    B = 'B',
}
export enum ExplicitEnum {
    C = 'C',
    D = 'D',
}
