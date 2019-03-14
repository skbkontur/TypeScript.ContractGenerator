
export type NamingRootType = {
    allCaps?: null | AllCaps;
    smallFirstLetter?: null | SmallFirstLetter;
    smallSecondLetter?: null | SmallSecondLetter;
    smallThirdLetter?: null | SmallThirdLetter;
    smallFourthLetter?: null | SmallFourthLetter;
    abc?: null | string;
    abCd?: null | string;
    abcD?: null | string;
    aBcd?: null | string;
    b: number;
    nb?: null | number;
    mySQLType?: null | string;
};
export type AllCaps = {
    a?: null | string;
    ab?: null | string;
    abc?: null | string;
    abcd?: null | string;
    abcde?: null | string;
};
export type SmallFirstLetter = {
    a?: null | string;
    aB?: null | string;
    aBC?: null | string;
    aBCD?: null | string;
    aBCDE?: null | string;
};
export type SmallSecondLetter = {
    ab?: null | string;
    abC?: null | string;
    abCD?: null | string;
    abCDE?: null | string;
};
export type SmallThirdLetter = {
    aBc?: null | string;
    aBcD?: null | string;
    aBcDE?: null | string;
};
export type SmallFourthLetter = {
    abCd?: null | string;
    abCdE?: null | string;
};
