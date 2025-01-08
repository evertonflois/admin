import { AfterViewInit, ChangeDetectorRef, Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { CrudBase } from '../../_common/crud-base';
import { AuthService, BreadcrumbService } from 'src/app/_services';
import { Router } from '@angular/router';
import { SubscriberService, ProfileService, UserService } from 'src/app/_services/authorization';
import { Table } from 'primeng/table';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { NgForm } from '@angular/forms';
import { PsSelectComponent } from '../../_structure/controls/ps-select/ps-select.component';
import { TranslateService } from '@ngx-translate/core';
import { PsToolbarComponent } from '../../_structure/controls/ps-toolbar/ps-toolbar.component';
declare var $: any;

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
  providers: [UserService, SubscriberService, ProfileService],
})
export class UserComponent extends CrudBase implements OnInit, AfterViewInit {
  @ViewChild("toolbar") 
  private _toolbar!: PsToolbarComponent;

  @ViewChild("dt") 
  private _dt!: Table;

  @ViewChild("form") 
  private _form!: NgForm;

  @ViewChildren(PsSelectComponent) 
  private _selects!: QueryList<PsSelectComponent>;

  transaction = "User";  

  constructor(public authService: AuthService, 
              public breadcrumbService: BreadcrumbService, 
              public router: Router,              
              public msgService: MessageService,
              public confirmationService: ConfirmationService,
              public cd: ChangeDetectorRef,
              public dataService: UserService,
              public SubscriberService: SubscriberService,              
              public ProfileService: ProfileService,
              public config: PrimeNGConfig, 
              public translateService: TranslateService
              )
  {
    super(authService, breadcrumbService, router, dataService, msgService, confirmationService, config, translateService);    
  }  

  ngOnInit(): void {
    super.onInit();     
  }

  ngAfterViewInit() {    
    this.sortField = "Login";
    this.toolbar = this._toolbar;
    this.dt = this._dt;  
    this.form = this._form;
    this.selects = this._selects;
    super.initGrid();
    super.afterViewInit();
    this.cd.detectChanges();    
  }    
  
  getCols() {
    return [
      { field: 'Login', header: 'Login' },
      { field: 'Name', header: 'Name' },
      { field: 'ProfileDescription', header: 'Profile' }      
  ];
  }

  getKey(row: any)  {
    return { SubscriberId : row.SubscriberId, Login : row.Login };
  }

  getPdfBody() {
      return this.gridRows.map(r => ([r.Login, r.Name, r.ProfileDescription]));
  }

  getExcelBody() {
      return this.gridRows.map(r => ({ Login: r.Login, Nome: r.Name, Profile: r.ProfileDescription }));
  }

}
