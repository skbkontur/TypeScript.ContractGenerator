/* eslint-disable */

export const url = String.raw;

export class ApiBase {
    public async makeGetRequest(url: string, body?: any): Promise<any> {
        const response = await fetch(url, {
            method: "GET",
        });
        return await response.json();
    }

    public async makePostRequest(url: string, body?: any): Promise<any> {
        const response = await fetch(url, {
            method: "POST",
            body: body && JSON.stringify(body),
        });
        const textResult = await response.text();
        if (textResult !== "") {
            return JSON.parse(textResult);
        }
    }

    public async makePutRequest(url: string, body?: any): Promise<any> {
        const response = await fetch(url, {
            method: "PUT",
            body: body && JSON.stringify(body),
        });
        const textResult = await response.text();
        if (textResult !== "") {
            return JSON.parse(textResult);
        }
    }

    public async makeDeleteRequest(url: string, body?: any): Promise<any> {
        const response = await fetch(url, {
            method: "DELETE",
            body: body && JSON.stringify(body),
        });
        const textResult = await response.text();
        if (textResult !== "") {
            return JSON.parse(textResult);
        }
    }
}
