import { Injectable } from '@angular/core';
import { RootService } from './root.service';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { AppConfig } from '../_models/app-config';
import { Observable, lastValueFrom } from 'rxjs';
import { ResponseMaintenance } from '../_models/responseMaintenance';

@Injectable({
  providedIn: 'root'
})
export class BaseService<T> extends RootService {

  constructor(
    public http: HttpClient,
    public messageService: MessageService,
    public config: AppConfig
  ) {
    super(http, messageService, config);    
  }

  setApiRoute(apiRoute: string){
    this.apiRoute = apiRoute;
    this.init();
  }
  
  getAll(params: any): Observable<T[]> {
    const options = { headers: this.headers, params };
    return this.http.get<T[]>(this.serviceUrl, options);
  }
  
  getCombo(params: any): Observable<T> {
    const options = { headers: this.headers, params };
    return this.http.get<T>(this.serviceUrl + '/GetCombo', options);
  }
  
  getList(params: any): Observable<T> {
    const options = { headers: this.headers, params };
    return this.http.get<T>(this.serviceUrl + '/GetList', options);
  }
  
  getById(params: any): Observable<T> {
    const options = { headers: this.headers, params };
    return this.http.get<T>(this.serviceUrl + '/GetById', options);
  }
  
  getDetails(params: any): Observable<T> {
    const options = { headers: this.headers, params };
    return this.http.get<T>(this.serviceUrl + '/GetDetails', options);
  }
  
  create(model: any): Promise<ResponseMaintenance> {
    return lastValueFrom(this.http
      .post(this.serviceUrl + '/Create', JSON.stringify(model), { headers: this.headers }))
      .then((res) => res as any)
      .catch((err) => this.handleError(err));
  }
  
  change(model: any): Promise<ResponseMaintenance> {
    return lastValueFrom(this.http
      .post(this.serviceUrl + '/Change', JSON.stringify(model), { headers: this.headers }))
      .then((res) => res as any)
      .catch((err) => this.handleError(err));
  }
  
  remove(model: any): Promise<ResponseMaintenance> {
    return this.http
      .post(this.serviceUrl + '/Remove', JSON.stringify(model), {
        headers: this.headers,
      })
      .toPromise()
      .then((res) => res as any)
      .catch((err) => this.handleError(err));
  }
  
  protected updateWithFuncao(funcao: string, model: any) {
    const url = this.serviceUrl + '?funcao=' + funcao.toUpperCase();
    return this.http
      .put(url, JSON.stringify(model), { headers: this.headers })
      .toPromise()
      .then((res) => res as any)
      .catch((err) => this.handleError(err));
  }
}
