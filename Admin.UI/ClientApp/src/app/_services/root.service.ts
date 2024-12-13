import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AppConfig } from '../_models/app-config';

@Injectable({
  providedIn: 'root'
})
export class RootService {

  public serviceUrl!: string;
  public endpoint!: string;
  public apiRoute!: string;
 
  protected headers = new HttpHeaders({ 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' });
 
  init() {
    this.serviceUrl = this.config.getConfig("apiUrl") + "/" + this.apiRoute;
  }
 
  constructor(
    public http: HttpClient,
    public messageService: MessageService,
    public config: AppConfig
  ) {    
  }
 
  getHeaders(): HttpHeaders {
    return this.headers;
  }
 
  handleError(result: any): Promise<any> {
    let msgErro = '';
    const err = result || result.error;
 
    if (err && typeof err === 'object') {
      if (err.errors && err.errors.length > 0 && typeof err.errors !== 'string') {
        let errorList = err.errors.reduce((accumulator: any, erro: any) => accumulator += erro.message + '. ', '');
        if (errorList.includes('undefined')) {
          errorList = err.errors.reduce((accumulator: any, erro: any) => accumulator += erro + '. ', '');
        }
        if (errorList !== '' && err.message !== undefined) {
          msgErro = err.message + ': ' + errorList;
        } else {
          msgErro = errorList;
        }
      } else if (err.error && err.error.code) {
        msgErro = err.error.description;
      } else if (result.message) {
        msgErro = result.message;
      } else {
        msgErro = JSON.stringify(result);
      }
    } else {
      msgErro = result;
    }
 
    this.messageService.add({ key: 'tst', severity: 'error', summary: 'Error Message', detail: msgErro });
    return Promise.reject(msgErro);
  }
}
