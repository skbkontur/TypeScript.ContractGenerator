export class ApiError extends Error {
    statusCode: number;
    serverStackTrace: null | undefined | string;
    serverErrorType: null | undefined | string;
    innerErrors: null | undefined | (any[]);
    responseAsText: string;

    constructor(
        responseAsText: string,
        message: string,
        statusCode: number,
        type?: null | undefined | string,
        stackTrace?: null | undefined | string,
        innerErrors?: null | undefined | (any[])
    ) {
        super(message);
        this.responseAsText = responseAsText;
        this.statusCode = statusCode;
        this.serverStackTrace = stackTrace;
        this.serverErrorType = type;
        this.innerErrors = innerErrors;
    }
}