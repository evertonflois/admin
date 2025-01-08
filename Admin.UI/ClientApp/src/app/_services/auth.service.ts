import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { User } from '../_models/user';
import { AppConfig } from '../_models/app-config';
import { SessionService } from './session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl?: string;
  private userSubject?: BehaviorSubject<User | null>;
  public user?: Observable<User | null>;

  constructor(
    private config: AppConfig,
    private router: Router,
    private http: HttpClient,
    private session: SessionService) { 
      this.apiUrl = this.config.getConfig('apiUrl');
      this.userSubject = new BehaviorSubject<User | null>(null);
      this.user = this.userSubject.asObservable();
      if (this.getUser())      
        this.startRefreshTokenTimer();
    }
   
    public getUser(){
      return this.getInfoStorage();
    }

    public getSubscriberId(){
      return this.getUser()?.SubscriberId;
    }

    login(login: string, password: string) {
      return this.http.post<any>(`${this.apiUrl}/Auth/Authenticate`, { login, password }, { withCredentials: true })
          .pipe(map(user => {              
                this.userSubject?.next(user);
                if (user.Success){
                  this.setInfoStorage(user);
                  this.startRefreshTokenTimer();                  
                }

                return user;              
          }));
    }

    logout() {
      this.cleanStorage();
      this.http.post<any>(`${this.apiUrl}/Auth/RevokeToken`, {}, { withCredentials: true }).subscribe();
      this.stopRefreshTokenTimer();
      this.userSubject?.next(null);
      window.location.reload();
  }

  refreshToken() {
      return this.http.post<any>(`${this.apiUrl}/Auth/RefreshToken`, {}, { withCredentials: true })
          .pipe(map((user) => {
              this.userSubject?.next(user);
              if (user.success){
                this.setTokenStorage(user);
                this.startRefreshTokenTimer(user);                  
              }
              return user;
          }));
  }

    // helper methods

    private refreshTokenTimeout?: any;

    private startRefreshTokenTimer(user?: any) {
        // parse json object from base64 encoded jwt token
        if (!user)
          user = this.getUser();
        const jwtBase64 = user!.Token!.split('.')[1];
        const jwtToken = JSON.parse(atob(jwtBase64));

        // set a timeout to refresh the token a minute before it expires
        const expires = new Date(jwtToken.exp * 1000);        
        const timeout = expires.getTime() - Date.now() - (60 * 1000);
        if (timeout < 0)
        {
          clearTimeout(this.refreshTokenTimeout);
          alert('Falha ao renovar token de autenticação, efetue novamente o login.')
          this.logout();          
        }     
        
        console.log("Refresh Token Timer ...");
        console.log(new Date());
        console.log("Timeout = " + timeout);
        
        if (this.refreshTokenTimeout)
          clearTimeout(this.refreshTokenTimeout);
        this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout);
    }

    private setInfoStorage(user: User)
    {     
      this.setTokenStorage(user);
      this.session.saveData('_user', JSON.stringify(user));
    }

    private setTokenStorage(user: User)
    {
      sessionStorage.setItem("_token", JSON.stringify({ token: user.Token } || {}));
    }

    private getInfoStorage()
    {
      if (sessionStorage.getItem('_user') != null)
        return JSON.parse(this.session.getData('_user') || "{}");

        return this.userSubject?.value;
    }

    private cleanStorage(){
      this.session.removeData('_user');
      this.session.removeData('_token');
    }
}
