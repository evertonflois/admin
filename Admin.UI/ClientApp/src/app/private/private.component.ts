import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';
import { AuthService } from '../_services/auth.service';
import { MenuAppService } from '../_services/menu-app.service';
// declare var $: any;

@Component({
  selector: 'app-private',
  templateUrl: './private.component.html',
  styleUrls: ['./private.component.css'],
})
export class PrivateComponent implements OnInit {

    topbarMenuActive?: boolean;
    overlayMenuActive?: boolean;
    staticMenuDesktopInactive?: boolean;
    staticMenuMobileActive?: boolean;
    menuClick?: boolean;
    topbarItemClick?: boolean;
    activeTopbarItem?: any;
    menuHoverActive?: boolean;
    rightPanelActive?: boolean;
    rightPanelClick?: boolean;
    megaMenuActive?: boolean;
    megaMenuClick?: boolean;
    usermenuActive?: boolean;
    usermenuClick?: boolean;
    activeProfileItem: any;
    configActive?: boolean;
    configClick?: boolean;

    intervalRefreshToken: any;

    constructor(
        public router: Router,
        private menuService: MenuAppService,
        public app: AppComponent,
        public authService: AuthService
    ) { }

  ngOnInit() {    
  }  

  onLayoutClick() {
    if (!this.topbarItemClick) {
        this.activeTopbarItem = null;
        this.topbarMenuActive = false;
    }

    if (!this.rightPanelClick) {
        this.rightPanelActive = false;
    }

    if (!this.megaMenuClick) {
        this.megaMenuActive = false;
    }

    if (!this.usermenuClick && this.isSlim()) {
        this.usermenuActive = false;
        this.activeProfileItem = null;
    }

    if (!this.menuClick) {
        if (this.isHorizontal() || this.isSlim()) {
            this.menuService.reset();
        }

        if (this.overlayMenuActive || this.staticMenuMobileActive) {
            this.hideOverlayMenu();
        }

        this.menuHoverActive = false;
    }

    if (this.configActive && !this.configClick) {
        this.configActive = false;
    }

    this.configClick = false;
    this.topbarItemClick = false;
    this.menuClick = false;
    this.rightPanelClick = false;
    this.megaMenuClick = false;
    this.usermenuClick = false;
  }

  onMenuButtonClick(event: any) {
      this.menuClick = true;
      this.topbarMenuActive = false;

      if (this.app.layoutMode === 'overlay') {
          this.overlayMenuActive = !this.overlayMenuActive;
      } else {
          if (this.isDesktop()) {
              this.staticMenuDesktopInactive = !this.staticMenuDesktopInactive;
          } else {
              this.staticMenuMobileActive = !this.staticMenuMobileActive;
          }
      }

      event.preventDefault();
  }

  onMenuClick($event: any) {
      this.menuClick = true;
  }

  onTopbarMenuButtonClick(event: any) {
      this.topbarItemClick = true;
      this.topbarMenuActive = !this.topbarMenuActive;

      this.hideOverlayMenu();

      event.preventDefault();
  }

  onTopbarItemClick(event: any, item: any) {
      this.topbarItemClick = true;

      if (this.activeTopbarItem === item) {
          this.activeTopbarItem = null;
      } else {
          this.activeTopbarItem = item;
      }

      event.preventDefault();
  }

  onTopbarSubItemClick(event: any) {
      event.preventDefault();
  }

  onRightPanelButtonClick(event: any) {
      this.rightPanelClick = true;
      this.rightPanelActive = !this.rightPanelActive;
      event.preventDefault();
  }

  onRightPanelClick() {
      this.rightPanelClick = true;
  }

  onRightPanelItemClick(event: any) {
    event.preventDefault();
}

  onMegaMenuClick() {
      this.megaMenuClick = true;
  }

  onConfigButtonClick(event: any) {
    this.configActive = !this.configActive;
    this.configClick = true;
    event.preventDefault();
  }

  hideOverlayMenu() {
      this.overlayMenuActive = false;
      this.staticMenuMobileActive = false;
  }

  onRippleChange(event: any) {
      this.app.ripple = event.checked;
  }

  onConfigClick(event: any) {
      this.configClick = true;
  }

  isDesktop() {
      return window.innerWidth > 1024;
  }

  isHorizontal() {
      return this.app.layoutMode === 'horizontal';
  }

  isSlim() {
      return this.app.layoutMode === 'slim';
  }

  isOverlay() {
      return this.app.layoutMode === 'overlay';
  }

}
