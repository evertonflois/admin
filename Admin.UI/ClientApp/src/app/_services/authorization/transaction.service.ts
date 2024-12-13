import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { Transaction } from 'src/app/_models/authorization';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { AppConfig } from 'src/app/_models/app-config';

@Injectable({
  providedIn: 'root'
})
export class TransactionService extends BaseService<Transaction> {

  constructor(
    public http: HttpClient,    
    public messageService: MessageService,
    public config: AppConfig
  ) {
    super(http, messageService, config);
    this.setApiRoute("Transaction");
   }
}
