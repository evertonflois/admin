import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { Profile } from 'src/app/_models/authorization';
import { HttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { AppConfig } from 'src/app/_models/app-config';

@Injectable({
  providedIn: 'root'
})
export class ProfileService extends BaseService<Profile> {

  constructor(
    public http: HttpClient,
    public messageService: MessageService,
    public config: AppConfig
  ) {
    super(http, messageService, config);
    this.setApiRoute("Profile");
   }
}
