import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router, 
              private authService: AuthService) {}

  // tslint:disable-next-line:typedef
  canActivate() {
    const user = this.authService?.getUser();
    if (user)
      return true;    

    this.router.navigate(['/login/auth']);
    return false;
  }
}
