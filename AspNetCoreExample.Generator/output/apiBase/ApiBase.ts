/* eslint-disable */

interface IParamsMap {
    [key: string]: null | undefined | number | string | any[] | boolean;
}

export class ApiBase {
    public async makeGetRequest(url: string, queryParams: IParamsMap, body: any): Promise<any> {
        const response = await fetch(this.getUrl(url, queryParams), {
            method: "GET",
        });
        return await response.json();
    }

    public async makePostRequest(url: string, queryParams: IParamsMap, body: any): Promise<any> {
        const response = await fetch(this.getUrl(url, queryParams), {
            method: "POST",
            body: JSON.stringify(body),
        });
        const textResult = await response.text();
        if (textResult !== "") {
            return JSON.parse(textResult);
        }
    }

    public async makePutRequest(url: string, queryParams: IParamsMap, body: any): Promise<any> {
        const response = await fetch(this.getUrl(url, queryParams), {
            method: "PUT",
            body: JSON.stringify(body),
        });
        const textResult = await response.text();
        if (textResult !== "") {
            return JSON.parse(textResult);
        }
    }

    public async makeDeleteRequest(url: string, queryParams: IParamsMap, body: any): Promise<any> {
        const response = await fetch(this.getUrl(url, queryParams), {
            method: "DELETE",
            body: JSON.stringify(body),
        });
        const textResult = await response.text();
        if (textResult !== "") {
            return JSON.parse(textResult);
        }
    }

    public getUrl(url: string, params?: IParamsMap): string {
        return url + this.createQueryString(params);
    }

    public createQueryString(params: any): string {
        if (params == null) {
            return "";
        }
        return `${new URLSearchParams(params)}`;
    }
}
