import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { AuthService, BreadcrumbService } from 'src/app/_services';
import { CrudBase } from '../../_common/crud-base';
import { ProfilePermissionsComponent } from './profile-permissions/profile-permissions.component';
import { TabsService } from 'src/app/_services/tabs.service';
import { Subscription } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-profile',  
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent extends CrudBase implements OnInit, OnDestroy {
  @ViewChild(ProfilePermissionsComponent)
  ProfilePermissionsComponent!: ProfilePermissionsComponent;
  
  activeIndex: number = 0;
  profileSelected!: any;
  tabsSubscription!: Subscription;
  
  transaction = "706";

  constructor(public authService: AuthService, 
    public breadcrumbService: BreadcrumbService, 
    public router: Router,    
    public msgService: MessageService,
    public confirmationService: ConfirmationService,
    public tabsService: TabsService,
    public config: PrimeNGConfig, 
    public translateService: TranslateService
    )
  {
    super(authService, breadcrumbService, router, {}, msgService, confirmationService, config, translateService);
    this.tabsSubscription = this.tabsService.getData().subscribe((data) => {
      this.activeIndex = data;
    });
  }

  ngOnInit(): void {
    super.onInit(); 
  }

  ngOnDestroy(): void {
    this.tabsSubscription.unsubscribe();
  }

  onprofileSelected(event: any){
    this.profileSelected = event.data;    
  }

  onTabViewChanged(event: any) {
    if (event.index == 1)
      this.ProfilePermissionsComponent.loadPermissions(this.profileSelected);
  }
}
