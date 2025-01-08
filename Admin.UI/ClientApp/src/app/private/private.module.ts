import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';


import { PrivateRoutingModule } from './private-routing.module';
import { TabViewModule } from 'primeng/tabview';
import { BadgeModule } from 'primeng/badge';
import { RadioButtonModule } from 'primeng/radiobutton';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastModule } from 'primeng/toast';
import { MessagesModule } from 'primeng/messages';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { ContextMenuModule } from 'primeng/contextmenu';
import { MegaMenuModule } from 'primeng/megamenu';
import { PanelMenuModule } from 'primeng/panelmenu';
import { SlideMenuModule } from 'primeng/slidemenu';
import { TabMenuModule } from 'primeng/tabmenu';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { ButtonModule } from 'primeng/button';
import { StyleClassModule } from 'primeng/styleclass';
import { RippleModule } from 'primeng/ripple';
import { ToolbarModule } from 'primeng/toolbar';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { InputSwitchModule } from 'primeng/inputswitch';
import { ProgressBarModule } from 'primeng/progressbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ToggleButtonModule } from 'primeng/togglebutton';


import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from '../_services/breadcrumb.service';
import { TabsService } from '../_services/tabs.service';

// PrimeNG Components
import { PrivateComponent } from './private.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TopBarComponent } from './_structure/top-bar/top-bar.component';
import { MenuComponent } from './_structure/menu/menu.component';
import { MenuitemComponent } from './_structure/menuitem/menuitem.component';
import { RightpanelComponent } from './_structure/rightpanel/rightpanel.component';
import { FooterComponent } from './_structure/footer/footer.component';
import { ConfigComponent } from './_structure/config/config.component';
import { BreadcrumbComponent } from './_structure/breadcrumb/breadcrumb.component';
import { UserComponent } from './authorization/user/user.component';
import { PsTextboxComponent } from './_structure/controls/ps-textbox/ps-textbox.component';
import { PsSelectComponent } from './_structure/controls/ps-select/ps-select.component';
import { PsActiveComponent } from './_structure/controls/ps-active/ps-active.component';
import { ProfileComponent } from './authorization/profile/profile.component';
import { ProfileCrudComponent } from './authorization/profile/profile-crud/profile-crud.component';
import { ProfilePermissionsComponent } from './authorization/profile/profile-permissions/profile-permissions.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { PsToolbarComponent } from './_structure/controls/ps-toolbar/ps-toolbar.component';
import { PsRowinfoComponent } from './_structure/controls/ps-rowinfo/ps-rowinfo.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    PrivateComponent,
    DashboardComponent,
    AccessDeniedComponent,
    TopBarComponent,
    MenuComponent,
    MenuitemComponent,
    RightpanelComponent,
    BreadcrumbComponent,
    FooterComponent,
    ConfigComponent,
    UserComponent,
    PsTextboxComponent,
    PsSelectComponent,    
    PsActiveComponent,
    PsToolbarComponent,
    PsRowinfoComponent,
    ProfileComponent,
    ProfileCrudComponent,
    ProfilePermissionsComponent,
  ],
  imports: [      
    CommonModule,
    FormsModule,    
    PrivateRoutingModule,
    NgxSpinnerModule,
    ToastModule,
    MessagesModule,      
    TabViewModule,
    BadgeModule,
    RadioButtonModule,
    MenuModule,
    MenubarModule,
    ContextMenuModule,
    MegaMenuModule,
    PanelMenuModule,
    SlideMenuModule,
    TabMenuModule,
    TieredMenuModule,
    ButtonModule,
    StyleClassModule,
    RippleModule,
    ToolbarModule,
    TableModule,
    DropdownModule,
    InputSwitchModule,
    ProgressBarModule,
    ConfirmDialogModule,
    BreadcrumbModule,
    ToggleButtonModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
    },
      defaultLanguage: 'pt'
  })
  ],
  providers: [MessageService, BreadcrumbService, ConfirmationService, TabsService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PrivateModule { }
