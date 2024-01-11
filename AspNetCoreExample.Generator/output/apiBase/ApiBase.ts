/* eslint-disable */

export const url = String.raw;

export const request = async (method: string, url: string, body?: any): Promise<any> => {
    const response = await fetch(url, {
        method: method,
        body: body && JSON.stringify(body),
    });

    const textResult = await response.text();
    if (textResult !== "") {
        return JSON.parse(textResult);
    }
}