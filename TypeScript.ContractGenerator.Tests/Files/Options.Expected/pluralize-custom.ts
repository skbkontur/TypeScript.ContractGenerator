
export type EnumContainingRootType = {
    defaultEnum: DefaultEnum;
    nullableEnum?: null | DefaultEnum;
    explicitEnum: ExplicitEnum;
};
export type DefaultEnum = 'A' | 'B';
export const DefaultEnumItems = {
    ['A']: ('A') as DefaultEnum,
    ['B']: ('B') as DefaultEnum,
};
export type ExplicitEnum = 'C' | 'D';
export const ExplicitEnumItems = {
    ['C']: ('C') as ExplicitEnum,
    ['D']: ('D') as ExplicitEnum,
};
