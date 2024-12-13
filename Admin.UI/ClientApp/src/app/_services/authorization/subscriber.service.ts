import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { AppConfig } from 'src/app/_models/app-config';
import { Subscriber } from 'src/app/_models/authorization';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class SubscriberService extends BaseService<Subscriber> {

  constructor(
    public http: HttpClient,
    public messageService: MessageService,
    public config: AppConfig
  ) {
    super(http, messageService, config);
    this.setApiRoute("Subscriber");
   }
}
