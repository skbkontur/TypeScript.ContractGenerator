
export type EnumContainingRootType = {
    defaultEnum: DefaultEnum;
    nullableEnum?: null | DefaultEnum;
    explicitEnum: ExplicitEnum;
};
export type DefaultEnum = 'A' | 'B';
export const DefaultEnumItems = {
    ['A']: (('A'): DefaultEnum),
    ['B']: (('B'): DefaultEnum),
};
export type ExplicitEnum = 'C' | 'D';
export const ExplicitEnumItems = {
    ['C']: (('C'): ExplicitEnum),
    ['D']: (('D'): ExplicitEnum),
};
