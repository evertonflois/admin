import { Component, OnInit } from '@angular/core';
import { PrivateComponent } from '../../private.component';
import { AppComponent } from 'src/app/app.component';
import { UserPreferencesService } from 'src/app/_services/authorization';
import { UserPreferences } from 'src/app/_models/authorization';
import { AuthService } from 'src/app/_services';

@Component({
  selector: 'app-config',
  template: `
        <div class="layout-config" [ngClass]="{'layout-config-active': appMain.configActive}" (click)="appMain.onConfigClick($event)">
            <h5>Menu Type</h5>
            <div class="field-radiobutton">
                <p-radioButton name="layoutMode" value="static" [(ngModel)]="app.layoutMode" inputId="layoutMode1" (click)="changeLayoutMode()"></p-radioButton>
                <label for="layoutMode1">Static</label>
            </div>
            <div class="field-radiobutton">
                <p-radioButton name="layoutMode" value="overlay" [(ngModel)]="app.layoutMode" inputId="layoutMode2" (click)="changeLayoutMode()"></p-radioButton>
                <label for="layoutMode2">Overlay</label>
            </div>
            <div class="field-radiobutton">
                <p-radioButton name="layoutMode" value="slim" [(ngModel)]="app.layoutMode" inputId="layoutMode3" (click)="changeLayoutMode()"></p-radioButton>
                <label for="layoutMode3">Slim</label>
            </div>
            <div class="field-radiobutton">
                <p-radioButton name="layoutMode" value="horizontal" [(ngModel)]="app.layoutMode" inputId="layoutMode4" (click)="changeLayoutMode()"></p-radioButton>
                <label for="layoutMode4">Horizontal</label>
            </div>

            <hr />

            <h5>Menu Color</h5>
            <div class="field-radiobutton">
                <p-radioButton name="menuMode" value="light" [(ngModel)]="app.menuMode" inputId="menuMode1" (click)="changeLayoutMode()"></p-radioButton>
                <label for="menuMode1">Light</label>
            </div>
            <div class="field-radiobutton">
                <p-radioButton name="menuMode" value="dark" [(ngModel)]="app.menuMode" inputId="menuMode2" (click)="changeLayoutMode()"></p-radioButton>
                <label for="menuMode2">Dark</label>
            </div>
            <div class="field-radiobutton">
                <p-radioButton name="menuMode" value="gradient" [(ngModel)]="app.menuMode" inputId="menuMode3" (click)="changeLayoutMode()"></p-radioButton>
                <label for="menuMode3">Gradient</label>
            </div>            
        </div>
    `,
    providers: [UserPreferencesService]
})
export class ConfigComponent implements OnInit {
  specialThemes?: any[];

  themes?: any[];
  
  usuarioPreferencias?: UserPreferences;

  constructor(public appMain: PrivateComponent, public app: AppComponent, public authService: AuthService, public usuarioPrefenciasService: UserPreferencesService) {
    
  }

  ngOnInit() {

    const user = this.authService.getUser();
    const userPrefKey = { 
                SubscriberId: this.authService.getSubscriberId(),
                Login: user.Login
    };
    this.usuarioPrefenciasService.getDetails(userPrefKey).subscribe((res: UserPreferences) => {
        if (res)
        {
            this.app.layoutMode = res.MenuType;
            this.app.menuMode = res.MenuColor;
            this.setConfiguration();    
        }
        this.usuarioPreferencias = res;
        
    });

      this.themes = [
          {name: 'blue', color: '#0071bc'},
          {name: 'cyan', color: '#00bfe7'},
          {name: 'green', color: '#4AA564'},
          {name: 'yellow', color: '#F9C642'},
          {name: 'purple', color: '#6A4AA5'},
          {name: 'pink', color: '#9f4488'},
          {name: 'bluegrey', color: '#4B6D7E'},
          {name: 'teal', color: '#07A089'},
          {name: 'orange', color: '#fe875d'},
          {name: 'grey', color: '#5B616B'},
      ];

      this.specialThemes = [
          {name: 'cappuccino', image: 'cappuccino'},
          {name: 'montreal', image: 'montreal'},
          {name: 'hollywood', image: 'hollywood'},
          {name: 'peak', image: 'peak'},
          {name: 'alive', color1: '#CB356B', color2: '#BD3F32'},
          {name: 'emerald', color1: '#348F50', color2: '#56B4D3'},
          {name: 'ash', color1: '#606c88', color2: '#3f4c6b'},
          {name: 'noir', color1: '#4b6cb7', color2: '#182848'},
          {name: 'mantle', color1: '#514A9D', color2: '#24C6DC'},
          {name: 'predawn', color1: '#00223E', color2: '#FFA17F'},
      ];
  }

  changeTheme(theme: string) {
      this.app.theme = theme;

      this.changeStyleSheetsColor('theme-css', 'theme-' + theme + '.css');
      this.changeStyleSheetsColor('layout-css', 'layout-' + theme + '.css');
  }

  changeStyleSheetsColor(id: string, value: string) {
      const element = document.getElementById(id);
      const urlTokens: string[] = element?.getAttribute('href')?.split('/') || [];
      if (urlTokens.length > 0)
        urlTokens[urlTokens.length - 1] = value;

      const newURL = urlTokens.join('/');

      this.replaceLink(element, newURL);
  }

  isIE() {
      return /(MSIE|Trident\/|Edge\/)/i.test(window.navigator.userAgent);
  }

  replaceLink(linkElement: HTMLElement | null, href: string) {
      if (this.isIE()) {
          linkElement?.setAttribute('href', href);
      } else {
          const id = linkElement?.getAttribute('id');
          const cloneLinkElement: any = linkElement?.cloneNode(true);

          if (cloneLinkElement != null)
          {
            cloneLinkElement.setAttribute('href', href);
            cloneLinkElement.setAttribute('id', id + '-clone');
          }

          linkElement?.parentNode?.insertBefore(cloneLinkElement, linkElement.nextSibling);

          cloneLinkElement.addEventListener('load', () => {
              linkElement?.remove();
              cloneLinkElement.setAttribute('id', id);
          });
      }
  }

  onConfigButtonClick(event: any) {
      this.appMain.configActive = !this.appMain.configActive;
      this.appMain.configClick = true;
      event.preventDefault();
  }

  changeLayoutMode() {
    this.setConfiguration();

    const user = this.authService.getUser();
    const usuarioPreferenciasSave: UserPreferences = { 
                SubscriberId: this.authService.getSubscriberId(),
                Login: user.Login,
                MenuType: this.app.layoutMode,
                MenuColor: this.app.menuMode
    };

    (!this.usuarioPreferencias ? 
    this.usuarioPrefenciasService.create(usuarioPreferenciasSave) :
    this.usuarioPrefenciasService.change(usuarioPreferenciasSave)).then((res) =>
    {
        if (!this.usuarioPreferencias)
            this.usuarioPreferencias = usuarioPreferenciasSave;

        this.appMain.configClick = false;
    });
  }

  setConfiguration() {
    if (this.app.layoutMode == 'horizontal')
        this.app.profileMode = 'top';
  }


}
