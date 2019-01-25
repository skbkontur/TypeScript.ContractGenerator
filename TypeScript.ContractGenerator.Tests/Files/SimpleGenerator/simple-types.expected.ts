
export type SimpleRootType = {
    long: string;
    uLong: UInt64;
    int: number;
    uInt: UInt32;
    short: Int16;
    uShort: UInt16;
    bool: boolean;
    double: Double;
    float: Single;
    decimal: number;
    byte: Byte;
    sByte: SByte;
    char: Char;
    string?: null | string;
    dateTime: (Date | string);
    timeSpan: TimeSpan;
    guid: Guid;
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
export type Guid = {
};
