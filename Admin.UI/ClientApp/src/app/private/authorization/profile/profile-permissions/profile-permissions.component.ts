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
  perfil!: any;

  @ViewChild("rowInfo") 
  private _rowInfo!: PsRowinfoComponent;
 
  @ViewChild("dt") 
  private _dt!: Table;

  selectedRows!: any;
  onlyAllowed!: boolean;
  onlyNotAllowed!: boolean;

  transaction = "706";

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
      CanChangePermissions: () => (this.permissions?.some(p => (p == 'ALTE_PERM')))
    };  
    this.sortField = "cd_trsc";    
  }
  
  ngAfterViewInit() {    
    this.dt = this._dt;
    this.cd.detectChanges();    
  }
  
  getCols() {
    return [
      { field: 'cd_trsc', header: 'Código' },      
      { field: 'ds_trsc', header: 'Transação' },
      { field: 'acoes', header: 'Ações' }      
  ];
  }

  loadPermissions(perfil: any){
    this.perfil = perfil;
    this.setRowInfo();
    this.defaultFilters = {
      cd_asnt: { value: perfil.cd_asnt },
      cd_perf: { value: perfil.cd_perf },
    };
    super.initGrid();    
  }

  setRowInfo() {
    this._rowInfo.selectedRowInfo = [{ label: "Perfil", value: `${this.perfil.cd_perf} - ${this.perfil.ds_perf}` }];
  }

  getKey(row: any)  {
    return { cd_asnt : row.cd_asnt, cd_perf : row.cd_perf };
  }

  getPdfBody() {
      return this.gridRows.map(r => ([r.cd_trsc, r.ds_trsc, r.acoes.map((a: any) => (a.ds_acao))]));
  }

  getExcelBody() {
    return this.gridRows.map(r => ({ "Código": r.cd_trsc, "Transação": r.ds_trsc, "Ações": r.acoes.map((a: any) => a.ds_acao).join(',') }))
  }

  onRowSelect(event: any) {    
    super.onRowSelect(event);    
  }

  onGridLoaded() {
    this.selectedRows = this.gridRows.filter(r => r.fl_perm );
    $(this.dt.el.nativeElement).find("thead tr:eq(0) th:eq(1)").css("left", "34px");
    $(this.dt.el.nativeElement).find("thead tr:eq(1) th:eq(1)").css("left", "34px");
    $(this.dt.el.nativeElement).find("thead tr:eq(0) th:eq(2)").css("left", "212px");
    $(this.dt.el.nativeElement).find("thead tr:eq(1) th:eq(2)").css("left", "212px");
  }

  changeAllowedFilter(disabled: boolean) {
    if (disabled)
      return;

    if (this.onlyAllowed || this.onlyNotAllowed)
      this.defaultFilters.fl_perm = { value : this.onlyAllowed ? 'S' : this.onlyNotAllowed ? 'N' : null };
    else
      delete this.defaultFilters.fl_perm;

    super.initGrid();
  }

  change() {
    
    for (let index = 0; index < this.gridRows.length; index++) {
      const row = this.gridRows[index];      
      row.fl_perm = this.selectedRows.indexOf(row) != -1;      
    }

    this.dataService.change(this.gridRows)
        .then((res: any) => {    
            this.msgService.add({ key: 'tst', severity: res.code == 0 ? 'success' : 'error', summary: 'Atenção!', detail: res.description });
    
            if (res.code == 0)
            this.back(true);
        });
  }

  backTab() {
    this.tabsService.setData(0);
  }
}
