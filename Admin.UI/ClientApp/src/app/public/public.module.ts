import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { NgxSpinnerModule } from 'ngx-spinner';
import { ReactiveFormsModule } from '@angular/forms';

import { PublicRoutingModule } from './public-routing.module';
import { LoginComponent } from './login/login.component';
import { PublicComponent } from './public.component';

//PrimeNG Modules
import {ButtonModule} from 'primeng/button';
import {CheckboxModule} from 'primeng/checkbox';
import {InputTextModule} from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@NgModule({
  declarations: [
    LoginComponent,    
    PublicComponent
  ],
  imports: [
    CommonModule,
    PublicRoutingModule,
    ButtonModule,
    CheckboxModule,
    InputTextModule,
    ReactiveFormsModule,   
    ToastModule, 
    // NgxSpinnerModule
  ],
  providers: [MessageService]
})
export class PublicModule { }
