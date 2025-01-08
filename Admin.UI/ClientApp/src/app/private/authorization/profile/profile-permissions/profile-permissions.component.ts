import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { Table } from 'primeng/table';
import { AuthService, BreadcrumbService } from 'src/app/_services';
import { TransactionService } from 'src/app/_services/authorization';
import { TabsService } from 'src/app/_services/tabs.service';
import { CrudBase } from 'src/app/private/_common/crud-base';
import { PsRowinfoComponent } from 'src/app/private/_structure/controls/ps-rowinfo/ps-rowinfo.component';
declare var $: any;

@Component({
  selector: 'app-profile-permissions',
  templateUrl: './profile-permissions.component.html',
  styleUrl: './profile-permissions.component.css',
  providers: [TransactionService]
})
export class ProfilePermissionsComponent extends CrudBase implements OnInit, AfterViewInit {
  
  @Input()
  profile!: any;

  @ViewChild("rowInfo") 
  private _rowInfo!: PsRowinfoComponent;
 
  @ViewChild("dt") 
  private _dt!: Table;

  selectedRows!: any;
  onlyAllowed!: boolean;
  onlyNotAllowed!: boolean;

  transaction = "Profile";

  constructor(public authService: AuthService, 
    public breadcrumbService: BreadcrumbService, 
    public router: Router,    
    public msgService: MessageService,
    public confirmationService: ConfirmationService,
    public cd: ChangeDetectorRef,
    public config: PrimeNGConfig, 
    public translateService: TranslateService,
    public dataService: TransactionService,
    public tabsService: TabsService    
    )
  {
    super(authService, breadcrumbService, router, {}, msgService, confirmationService, config, translateService);
  }  

  ngOnInit(): void {
    super.onInit(); 
    this.can = {
      Back: () => (true),
      CanChangePermissions: () => (this.permissions?.some(p => (p == 'CHANGE_PERM')))
    };  
    this.sortField = "TransactionCode";    
  }
  
  ngAfterViewInit() {    
    this.dt = this._dt;
    this.cd.detectChanges();    
  }
  
  getCols() {
    return [
      { field: 'TransactionCode', header: 'Code' },      
      { field: 'Description', header: 'Description' },
      { field: 'Actions', header: 'Actions' }      
  ];
  }

  loadPermissions(profile: any){
    this.profile = profile;
    this.setRowInfo();
    this.defaultFilters = {
      SubscriberId: { value: profile.SubscriberId },
      ProfileCode: { value: profile.ProfileCode },
    };
    super.initGrid();    
  }

  setRowInfo() {
    this._rowInfo.selectedRowInfo = [{ label: "Profile", value: `${this.profile.ProfileCode} - ${this.profile.Description}` }];
  }

  getKey(row: any)  {
    return { SubscriberId : row.SubscriberId, ProfileCode : row.ProfileCode };
  }

  getPdfBody() {
      return this.gridRows.map(r => ([r.TransactionCode, r.Description, r.Actions.map((a: any) => (a.Description))]));
  }

  getExcelBody() {
    return this.gridRows.map(r => ({ "Código": r.TransactionCode, "Transação": r.Description, "Ações": r.Actions.map((a: any) => a.Description).join(',') }))
  }

  onRowSelect(event: any) {    
    super.onRowSelect(event);    
  }

  onGridLoaded() {
    this.selectedRows = this.gridRows.filter(r => r.FlagPermission );
    $(this.dt.el.nativeElement).find("thead tr:eq(0) th:eq(1)").css("left", "34px");
    $(this.dt.el.nativeElement).find("thead tr:eq(1) th:eq(1)").css("left", "34px");
    $(this.dt.el.nativeElement).find("thead tr:eq(0) th:eq(2)").css("left", "212px");
    $(this.dt.el.nativeElement).find("thead tr:eq(1) th:eq(2)").css("left", "212px");
  }

  changeAllowedFilter(disabled: boolean) {
    if (disabled)
      return;

    if (this.onlyAllowed || this.onlyNotAllowed)
      this.defaultFilters.FlagPermission = { value : this.onlyAllowed ? 'Y' : this.onlyNotAllowed ? 'N' : null };
    else
      delete this.defaultFilters.FlagPermission;

    super.initGrid();
  }

  change() {
    
    for (let index = 0; index < this.gridRows.length; index++) {
      const row = this.gridRows[index];      
      row.FlagPermission = this.selectedRows.indexOf(row) != -1;      
    }

    this.dataService.change(this.gridRows)
        .then((res: any) => {    
            this.msgService.add({ key: 'tst', severity: res.code == 0 ? 'success' : 'error', summary: 'Attention!', detail: res.description });
    
            if (res.code == 0)
            this.back(true);
        });
  }

  backTab() {
    this.tabsService.setData(0);
  }
}
