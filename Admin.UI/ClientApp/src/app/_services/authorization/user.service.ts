import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { AppConfig } from 'src/app/_models/app-config';
import { User } from 'src/app/_models/authorization';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseService<User> {
    constructor(
    public http: HttpClient,
    public messageService: MessageService,
    public config: AppConfig
  ) {
    super(http, messageService, config);
    this.setApiRoute("User");
   }
}
