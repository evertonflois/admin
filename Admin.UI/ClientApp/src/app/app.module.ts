import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule, HashLocationStrategy, LocationStrategy, registerLocaleData } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import ptBr from '@angular/common/locales/pt';
registerLocaleData(ptBr);
import { NgxSpinnerModule } from 'ngx-spinner';

// Application Components
import { AppComponent } from './app.component';
import { AppConfig } from './_models/app-config';
import { AuthGuard } from './_helpers/guard/auth-guard';
import { JwtInterceptor } from './_helpers/jwt/jwt.interceptor';
import { AuthService } from './_services/auth.service';
import { ToastModule } from 'primeng/toast';
import { MessagesModule } from 'primeng/messages';

// services
//import { AuthenticationService } from './shared/_services';
import { SessionService } from './_services/session.service';

export function initConfig(config: AppConfig) {
  return () => config.load();
}

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    BrowserModule,
    NgxSpinnerModule,
    ToastModule,
    MessagesModule,    
  ],
  declarations: [
    AppComponent
  ],
  providers: [
    AppConfig,
    AuthGuard,    
    AuthService,
    SessionService,
    {
      provide: APP_INITIALIZER,
      useFactory: initConfig,
      deps: [AppConfig],
      multi: true
    },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
