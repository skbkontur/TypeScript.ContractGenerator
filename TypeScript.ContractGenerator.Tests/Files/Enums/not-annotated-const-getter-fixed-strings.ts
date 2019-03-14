
export type EnumWithConstGetterContainingRootType = {
    defaultEnum: DefaultEnum;
    explicitEnum: ExplicitEnum;
};
export type DefaultEnum = 'A' | 'B';
export const DefaultEnums = {
    ['A']: ('A') as DefaultEnum,
    ['B']: ('B') as DefaultEnum,
};
export type ExplicitEnum = 'C' | 'D';
export const ExplicitEnums = {
    ['C']: ('C') as ExplicitEnum,
    ['D']: ('D') as ExplicitEnum,
};
