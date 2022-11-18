
export type SimpleRootType = {
    long: number;
    uLong: number;
    int: number;
    uInt: number;
    short: number;
    uShort: number;
    bool: boolean;
    double: number;
    float: number;
    decimal: number;
    byte: number;
    sByte: number;
    char: string;
    string?: null | string;
    dateTime: (Date | string);
    dateTimeOffset: (Date | string);
    timeSpan: (number | string);
    guid: string;
};
