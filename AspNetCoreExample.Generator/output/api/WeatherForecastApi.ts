// tslint:disable
// TypeScriptContractGenerator's generated content
import { WeatherForecast } from './../DataTypes/WeatherForecast';
import { Guid } from './../DataTypes/Guid';
import { ApiBase } from './../ApiBase/ApiBase';
import { url } from './../ApiBase/ApiBase';

export class WeatherForecastApi extends ApiBase implements IWeatherForecastApi {
    get(): Promise<WeatherForecast[]> {
        return this.makeGetRequest(url`/WeatherForecast`);
    }

    update(city: string, forecast: WeatherForecast): Promise<void> {
        return this.makePostRequest(url`/WeatherForecast/Update/${city}`, forecast);
    }

    reset(seed: number): Promise<void> {
        return this.makePostRequest(url`/Reset?seed=${seed}`);
    }

    urlForDownload(city: string): string {
        return url`/WeatherForecast/${city}`;
    }

    urlForGetStreetView(street: string, useGoogleImages: boolean): string {
        return url`/WeatherForecast/${street}/view?useGoogleImages=${useGoogleImages}`;
    }

    newGuid(): Promise<Guid> {
        return this.makeGetRequest(url`/WeatherForecast/none`);
    }

};
export interface IWeatherForecastApi {
    get(): Promise<WeatherForecast[]>;
    update(city: string, forecast: WeatherForecast): Promise<void>;
    reset(seed: number): Promise<void>;
    urlForDownload(city: string): string;
    urlForGetStreetView(street: string, useGoogleImages: boolean): string;
    newGuid(): Promise<Guid>;
}
