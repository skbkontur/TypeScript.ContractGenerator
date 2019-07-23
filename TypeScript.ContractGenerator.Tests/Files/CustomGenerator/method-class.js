
export type CommonType = {
    count: number;
    string?: null | string;
};
export interface MethodRootType {
    Get(): null | CommonType;
    GetNotNull(): CommonType;
}
