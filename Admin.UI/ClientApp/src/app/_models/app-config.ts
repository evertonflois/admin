import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import * as moment from 'moment';

@Injectable()
export class AppConfig {
  private config: any = null;
  private env: any = null;

  constructor(private http: HttpClient) {}

  /**
   * Use to get the data found in the second file (config file)
   */
  public getConfig(key: any) {
    return this.config[key];
  }

  /**
   * Use to get the data found in the first file (env file)
   */
  public getEnv(key: any) {
    return this.env[key];
  }

  /**
   * This method:
   *   a) Loads "env.json" to get the current working environment (e.g.: 'production', 'development')
   *   b) Loads "config.[env].json" to get all env's variables (e.g.: 'config.development.json')
   */
  public load() {
    const configFolder = 'assets/config/';
    const hash = moment().format('DDMMYYHHmmss');
    return new Promise((resolve, reject) => {
      this.http
        .get(configFolder + 'env.json?' + hash)
        .pipe(
          map(res => res),
          catchError(error => {
            console.log('Configuration file \'env.json\' could not be read');
            resolve(true);
            // tslint:disable: deprecation
            return throwError(() => new Error(error.json().error || 'Server error'));
          })
        )
        .subscribe((envResponse: any) => {
          this.env = envResponse;
          let request: any = null;

          switch (envResponse.env) {
            case 'production':
              {
                request = this.http.get(
                  configFolder + 'config.' + envResponse.env + '.json?' + hash
                );
              }
              break;

            case 'qa':
              {
                request = this.http.get(
                  configFolder + 'config.' + envResponse.env + '.json?' + hash
                );
              }
              break;

            case 'development':
              {
                request = this.http.get(
                  configFolder + 'config.' + envResponse.env + '.json?' + hash
                );
              }
              break;

            case 'default':
              {
                console.error('Environment file is not set or invalid');
                resolve(true);
              }
              break;
          }

          if (request) {
            request
              .pipe(
                map(res => res),
                catchError(error => {
                  console.error(
                    'Error reading ' + envResponse.env + ' configuration file'
                  );
                  resolve(error);
                  return throwError(() => new Error(error.json().error || 'Server error'));
                })
              )
              .subscribe((responseData: any) => {
                this.config = responseData;
                resolve(true);
              });
          } else {
            console.error('Env config file \'env.json\' is not valid');
            resolve(true);
          }
        });
    });
  }
}
