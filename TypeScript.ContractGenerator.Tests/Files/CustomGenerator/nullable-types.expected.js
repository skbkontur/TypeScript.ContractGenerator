
export type SimpleNullableRootType = {
    long?: null | string;
    uLong?: null | UInt64;
    int?: null | number;
    uInt?: null | UInt32;
    short?: null | Int16;
    uShort?: null | UInt16;
    bool?: null | boolean;
    double?: null | Double;
    decimal?: null | number;
    float?: null | Single;
    byte?: null | Byte;
    sByte?: null | SByte;
    char?: null | Char;
    dateTime?: null | (Date | string);
    timeSpan?: null | TimeSpan;
    guid?: null | string;
};
export type UInt64 = {
};
export type UInt32 = {
};
export type Int16 = {
};
export type UInt16 = {
};
export type Double = {
};
export type Single = {
};
export type Byte = {
};
export type SByte = {
};
export type Char = {
};
export type TimeSpan = {
    ticks: string;
    days: number;
    hours: number;
    milliseconds: number;
    minutes: number;
    seconds: number;
    totalDays: Double;
    totalHours: Double;
    totalMilliseconds: Double;
    totalMinutes: Double;
    totalSeconds: Double;
};
