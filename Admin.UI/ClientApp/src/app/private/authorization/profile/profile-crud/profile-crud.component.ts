import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, OnInit, Output, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { Table } from 'primeng/table';
import { AuthService, BreadcrumbService } from 'src/app/_services';
import { SubscriberService, ProfileService } from 'src/app/_services/authorization';
import { CrudBase } from 'src/app/private/_common/crud-base';
import { PsSelectComponent } from 'src/app/private/_structure/controls/ps-select/ps-select.component';
import { PsToolbarComponent } from 'src/app/private/_structure/controls/ps-toolbar/ps-toolbar.component';

@Component({
  selector: 'app-profile-crud',
  templateUrl: './profile-crud.component.html',
  styleUrl: './profile-crud.component.css',
  providers: [ProfileService, SubscriberService]
})
export class ProfileCrudComponent extends CrudBase implements OnInit, AfterViewInit {
  @ViewChild("toolbar") 
  private _toolbar!: PsToolbarComponent;

  @ViewChild("dt") 
  private _dt!: Table;

  @ViewChild("form") 
  private _form!: NgForm;

  @ViewChildren(PsSelectComponent) 
  private _selects!: QueryList<PsSelectComponent>;

  @Output()
  profileSelected = new EventEmitter();

  transaction = "Profile";

  constructor(public authService: AuthService, 
    public breadcrumbService: BreadcrumbService, 
    public router: Router,    
    public msgService: MessageService,
    public confirmationService: ConfirmationService,
    public cd: ChangeDetectorRef,
    public config: PrimeNGConfig, 
    public translateService: TranslateService,
    public dataService: ProfileService,
    public SubscriberService: SubscriberService)
  {
    super(authService, breadcrumbService, router, {}, msgService, confirmationService, config, translateService);
  }  

  ngOnInit(): void {
    super.onInit();
    this.sortField = "ProfileCode";
  }
  
  ngAfterViewInit() {    
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
      { field: 'ProfileCode', header: 'Code' },      
      { field: 'Description', header: 'Description' }      
  ];
  }

  getKey(row: any)  {
    return { SubscriberId : row.SubscriberId, ProfileCode : row.ProfileCode };
  }

  getPdfBody() {
      return this.gridRows.map(r => ([r.ProfileCode, r.Description]));
  }

  getExcelBody() {
      return this.gridRows.map(r => ({ "Code": r.ProfileCode, "Description": r.Description }));
  }

  onRowSelect(event: any) {    
    super.onRowSelect(event);
    this.profileSelected.emit(event);
  }
}
