// tslint:disable
// TypeScriptContractGenerator's generated content
import { WeatherForecast } from './../DataTypes/WeatherForecast';
import { ApiBase } from './../ApiBase/ApiBase';

export class WeatherForecastApi extends ApiBase implements IWeatherForecastApi {
    async get(): Promise<WeatherForecast[]> {
        return this.makeGetRequest(`/WeatherForecast`, {
            
        }, {
            
        });
    }

    async update(city: string, forecast: WeatherForecast): Promise<void> {
        return this.makePostRequest(`/WeatherForecast/Update/${city}`, {
            
        }, {
            ...forecast,
        });
    }

    async reset(seed: number): Promise<void> {
        return this.makePostRequest(`/Reset`, {
            ['seed']: seed,
        }, {
            
        });
    }

    getDownloadUrl(city: string): string {
        return `/WeatherForecast/${city}`;
    }

};
export interface IWeatherForecastApi {
    get(): Promise<WeatherForecast[]>;
    update(city: string, forecast: WeatherForecast): Promise<void>;
    reset(seed: number): Promise<void>;
    getDownloadUrl(city: string): string;
}
