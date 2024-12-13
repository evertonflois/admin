import { Component } from '@angular/core';
import { AppComponent } from 'src/app/app.component';
import { PrivateComponent } from '../../private.component';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-topbar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent {
  userName?: string;

  constructor(
    public app: AppComponent,
    public appMain: PrivateComponent,
    private authService: AuthService,)
    {
      this.userName = this.authService.getUser().cd_user;
    }

    logout() {
      this.authService.logout();
    }

    openPreferences($event: any){
      this.appMain.onConfigButtonClick($event);
    }
}
