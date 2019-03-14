
export type EnumWithConstGetterContainingRootType = {
    defaultEnum: DefaultEnum;
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
