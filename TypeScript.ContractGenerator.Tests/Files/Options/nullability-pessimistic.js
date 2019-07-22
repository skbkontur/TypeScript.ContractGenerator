
export type NullabilityModeRootType = {
    notNullString: string;
    canBeNullString?: null | string;
    maybeNotNullString?: null | string;
    maybeItemNotNullArray?: null | Array<null | string>;
};
