import { Injectable } from '@angular/core';
import { UserPreferences } from 'src/app/_models/authorization';
import { BaseService } from '../base.service';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { AppConfig } from 'src/app/_models/app-config';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService extends BaseService<UserPreferences> {

  constructor(
    public http: HttpClient,
    public messageService: MessageService,
    public config: AppConfig
  ) {
    super(http, messageService, config);
    this.setApiRoute("UserPreferences");
   }
}
