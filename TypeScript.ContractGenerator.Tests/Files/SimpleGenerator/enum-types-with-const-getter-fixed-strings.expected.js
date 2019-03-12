
export type EnumWithConstGetterContainingRootType = {
    defaultEnum: DefaultEnum;
    explicitEnum: ExplicitEnum;
};
export type DefaultEnum = 'A' | 'B';
export const DefaultEnums = {
    ['A']: (('A'): DefaultEnum),
    ['B']: (('B'): DefaultEnum),
};
export type ExplicitEnum = 'C' | 'D';
export const ExplicitEnums = {
    ['C']: (('C'): ExplicitEnum),
    ['D']: (('D'): ExplicitEnum),
};
