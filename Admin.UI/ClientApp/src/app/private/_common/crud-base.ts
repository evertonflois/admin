import { Injectable, QueryList } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { Table, TableLazyLoadEvent } from "primeng/table";
import { AuthService, BreadcrumbService } from "src/app/_services";
import { PsSelectComponent } from "../_structure/controls/ps-select/ps-select.component";
import * as FileSaver from 'file-saver';
import autoTable from 'jspdf-autotable'
import { ConfirmationService, MessageService, PrimeNGConfig } from "primeng/api";
import { TranslateService } from "@ngx-translate/core";
import { PsToolbarComponent } from "../_structure/controls/ps-toolbar/ps-toolbar.component";
declare var $: any;

interface Column {
    field: string;
    header: string;
    customExportHeader?: string;
  }

@Injectable()
export abstract class CrudBase {   

  breadCrumbItems?: string[];

  transaction?: string;
  permissions?: string[];
    
  toolbar!: PsToolbarComponent;
  dt!: Table;
  form!: NgForm;
  selects!: QueryList<PsSelectComponent>;
  gridStarted = false;
  defaultFilters: any;
  gridRows: any[] = [];
  totalRecords = 10;
  loading: boolean = false;
  page = 1;
  first = 0;
  rows = 10;
  sortField = "";
  selectedRow!: any;
  cols!: Column[];
  exportColumns!: any[];
  isSingleClick: Boolean = true;
  mode = "list";
  editing = false;
  model: any = { };  
  detail: any = { };
  serverModel: any = { };
  selectsLoaded = false;
  selectsLoadedCounter = 0;  
  showFormLoading = false;
  onlyActive!: boolean;
  onlyNotActive!: boolean;
  can: any = {
    Create: () => {},
    Change: () => {},
    Delete: () => {},
    Save: () => {},
    Clean: () => {},
    Back: () => {},
  };

    constructor(
        public authService: AuthService, 
        public breadcrumbService: BreadcrumbService,
        public router: Router,
        public dataService: any,
        public msgService: MessageService,
        public confirmationService: ConfirmationService,
        public config: PrimeNGConfig, 
        public translateService: TranslateService){
            this.defaultFilters = { SubscriberId : { value: this.authService.getSubscriberId() } };
    }

    onInit() {
        this.checkAuth();
        this.setBreadcrumb();        
        //this.translateService.setDefaultLang('pt');
    }
    
    afterViewInit() {
        //this.translate('pt');
        this.initCan();
    }

    changeActiveFilter(disabled: boolean) {
        if (disabled)
          return;    
    
        if (!this.defaultFilters)
          this.defaultFilters = {};
    
        if (this.onlyActive || this.onlyNotActive)
          this.defaultFilters.Active = { value : this.onlyActive ? 'Y' : this.onlyNotActive ? 'N' : null };
        else
          delete this.defaultFilters.Active;

        this.initGrid();
      }

    checkAuth() {
        const _user = this.authService.getUser();
        if (_user && _user.Transactions) {        
            const transaction = _user.Transactions.find((t: any) => t.Code == this.transaction);
            if (transaction)
                this.permissions = transaction?.Permissions;
            else
                this.router.navigateByUrl("/access-denied");
        }
    }

    translate(lang: string) {        
        this.translateService.use(lang);
        this.translateService
         .get('primeng')
         .subscribe(res => this.config.setTranslation(res));
      }

    setBreadcrumb() {
        if (!this.breadCrumbItems)
        {
            var menus = this.authService.getUser().Menu;
            this.findBreadcrumb(menus, this.router.url);
        }
        
        if (this.breadCrumbItems)
            this.breadcrumbService.setItems(this.breadCrumbItems.map((i) => ({
                label: i    
            })));
    }

    findBreadcrumb(arr: any[], val: string, r = false): any  {        
        for(let obj of arr){
            if (!r)
                this.breadCrumbItems = [];

            if (obj.routerLink && obj.routerLink.indexOf(val) != -1) {
                return obj;
            }           

            if(obj.items){
                let result = this.findBreadcrumb(obj.items, val, true);
                if (result) {
                    this.breadCrumbItems?.push(obj.label);
                    this.breadCrumbItems?.push(result.label);
                    return result;
                }                
            }
        }
        return undefined;
    };

    getTitle() {
        if (this.mode == "form"){
            if (this.editing)
              return "Edit record";
            else
              return "New record";
        }
    
        return "Record List";
      }

    initCan() {
        this.can.Create = () => (this.mode === 'list' && this.permissions?.some(p => (p == 'CREATE') ));
        this.can.Change = () => (this.mode === 'list' && this.permissions?.some(p => (p == 'CHANGE') ));
        this.can.Save = () => (this.mode == 'form' && (this.permissions?.some(p => ((p == 'CREATE' && !this.editing) || (p == 'CHANGE' && this.editing)))));
        this.can.Delete = () => (this.editing && this.permissions?.some(p => (p == 'REMOVE') ));
        this.can.Clean = () => (this.mode == 'form' && !this.editing);
        this.can.Back = () => (this.mode == 'form');
        if (this.toolbar)
            this.toolbar.setCan(this.can);
      }

  //##################################
  //########## Grid Begin ############
  //##################################
  initGrid() {
    if (!this.gridStarted)
    {
        this.cols = this.getCols();
        this.dt.columns = this.cols;
        this.exportColumns = this.cols.map((col) => col.header);    
        this.dt.lazy = true;    
        this.dt.styleClass = "p-datatable-sm";
        this.dt.tableStyle = { 'min-width': '65rem' };
        this.dt.paginator = true;
        this.dt.rows = this.rows;
        this.dt.showCurrentPageReport = true;
        this.dt.currentPageReportTemplate = this.getPageReportTemplate();
        this.dt.rowsPerPageOptions = [10, 20, 30];
        this.dt.totalRecords = this.totalRecords;
        this.dt.paginatorLocale = "pt-BR";    
        this.dt.selectionMode = !this.dt.selectionMode ? "single" : this.dt.selectionMode;

        this.dt.onLazyLoad.subscribe((evt) => {
        this.loadGridRows(evt);
        });

        this.dt.onPage.subscribe((evt) => {
        this.pageChange(evt);
        });

        this.dt.onFilter.subscribe((evt) => {
        this.onFilter(evt);
        });

        this.dt.onFilter.subscribe((evt) => {
            this.onFilter(evt);
        });

        if (this.dt.selectionMode == "single")
        {
            this.dt.onRowSelect.subscribe((evt) => {
            this.onRowSelect(evt);
            });

            this.dt.onRowUnselect.subscribe((evt) => {
                evt.data = undefined;
                this.onRowSelect(evt);
            });
        }
        this.gridStarted = true;
    }

    this.loadGridRows({ });    
  }

  getCols(): any[] {
    return [];
  }

  getRowCssClass(index: number, row: any){
    return (index % 2 == 1 ? 'odd' : '') + (row.Active == 'N' ? ' row-inactive' : '');
  }

  loadGridRows(event: TableLazyLoadEvent) {
    this.loading = true;

    const filters: any[] = [];
    if (event.filters || this.defaultFilters){
      let unionFilters = Object.assign({}, this.defaultFilters, event.filters);
      var keys = Object.keys(unionFilters);
      for (let index = 0; index < keys.length; index++) {
        const element = keys[index];        
        const filter: any = unionFilters[element];
        if (filter.value)
            filters.push({ "name": element, "value": filter.value, "matchMode": filter.matchMode });
      }
    }

    setTimeout(() => {
        const params: any = {
          filter: JSON.stringify(filters),
          sortField: event.sortField || this.sortField,
          sortOrder: event.sortOrder ? event.sortOrder == 1 ? 'asc' : 'desc' : 'asc',
          pageNumber: this.page,
          pageSize: this.rows
        };
        

        this.dataService.getAll(params).subscribe((res: any) => {
            this.gridRows = res;            
            this.totalRecords = res.length > 0 ? res[0].rowscount : 0;
            this.loading = false;
            this.dt.totalRecords = this.totalRecords;
            $(".p-paginator-current", $("#" + this.dt.id)).html(this.getPageReportTemplate());
            setTimeout(() => (this.onGridLoaded()), 1000);
        });
    }, 1000);
  }

  onGridLoaded() {   
  }
  
  onFilter(event: any) {
      this.reset();
  }  

  reset(fromButton = false) {
      this.first = 0;
      this.page = 1;

      if (fromButton)
        this.dt.reset();
  }

  pageChange(event: any) {
    this.first = event.first;
    this.rows = event.rows;
    this.page = event.first == 0 ? 1 : (event.first / event.rows) + 1;
  }  

    getPageReportTemplate(): string {
        const pageRecords = this.first + this.rows;
        return `Showing ${this.gridRows?.length > 0 ? this.first + 1 : 1} to ${this.totalRecords < pageRecords ? this.totalRecords : pageRecords} of ${this.totalRecords} records`;
    }

    exportPdf() {
        import('jspdf').then((jsPDF) => {
            
                const doc = new jsPDF.default('p', 'px', 'a4');
                autoTable(doc, {
                head: [this.exportColumns],
                body: this.getPdfBody(),
                })
                // (doc as any).autoTable(this.exportColumns, this.gridRows);
                doc.save('export.pdf');
            
        });
    }

    getKey(row: any) : any {
        return {  };
    }
    
    getPdfBody() : any[] {
        return [];
    }
    
    getExcelBody() : any[]  {
        return [];
    }

    exportExcel() {
        import('xlsx').then((xlsx) => {
            const worksheet = xlsx.utils.json_to_sheet(this.getExcelBody());
            const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] };
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
            this.saveAsExcelFile(excelBuffer, 'exportExcel');
        });
    }

    saveAsExcelFile(buffer: any, fileName: string): void {
        let EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
        let EXCEL_EXTENSION = '.xlsx';
        const data: Blob = new Blob([buffer], {
            type: EXCEL_TYPE
        });
        FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
    }

    onRowSelect(event: any) {    
    this.isSingleClick = true;
            setTimeout(()=>{
                if(this.isSingleClick)
                    this.selectedRow = event.data;            
            },250)
    }

    onRowDblClick(event: any, row: any) {  
    this.isSingleClick = false;
    this.edit(row);
    }
    //##################################
    //########## Grid End ##############
    //##################################


    //##################################
    //########## Form Begin ############
    //##################################

    new() {
        this.mode = "form";
        this.editing = false;
        this.clean();
        this.loadSelects();
    }
    
    editSelectedRow(){
        if (!this.selectedRow)
        {
        this.msgService.add({ key: 'tst', severity: 'warn', summary: 'Aviso!', detail: "Selecione uma linha para a edição." }); 
        return;
        }
        this.edit(this.selectedRow);
    }
  
    edit(row: any = null) {
        this.clean();
        this.mode = "form";
        this.editing = true;  
        this.showFormLoading = true;
    
        this.dataService.getDetails(this.getKey(row)).subscribe((res: any) => {
        this.showFormLoading = false;
        res = this.serverToModel(res);
        this.detail = res;
        if (this.selects.length > 0)
            this.loadSelects();
        else
            this.model = this.detail;
        });
    }
  
    serverToModel(res: any){
    
        if (res.Active)
            res.Active = res.Active == 'Y';
    
        return res;
    }
    
    save(event: any) {  
        this.form.onSubmit(event);
    }
  
    clean(){      
        this.form.resetForm();  
        this.selects.forEach(s => {
        s.value = undefined;
        });
        this.defaultValues();
    }
    
    defaultValues() {
        setTimeout(() => {
        this.selectsLoadedCounter = 0;
        this.detail = undefined;
        this.selectsLoaded = false;
        this.model = {
            Active : true
        }
        }, 50);
    }
    
    remove(event: Event){  
        this.confirmationService.confirm({
        target: event.target as EventTarget,
        message: 'Are you sure?',
        header: 'Confirm',
        icon: 'pi pi-info-circle',
        acceptButtonStyleClass:"p-button-danger p-button-text",
        rejectButtonStyleClass:"p-button-text p-button-text",
        acceptLabel: "Yes",
        rejectLabel: "No",
        acceptIcon:"none",
        rejectIcon:"none",
    
        accept: () => {
            this.modelToServer();
            this.dataService.remove(this.serverModel)
            .then((res: any) => {    
                this.msgService.add({ key: 'tst', severity: res.code == 0 ? 'success' : 'error', summary: 'Attention!', detail: res.description });
        
                if (res.code == 0)
                this.back(true);
            }).catch((error: any) => (this.msgService.add({ key: 'tst', severity: 'error', summary: 'Error!', detail: error.message })));
        },
        reject: () => {
            
        }
    });
    }
    
    back(loadGrid = false) {  
        this.mode = "list";
        this.editing = false;
    
        if (loadGrid)
        this.loadGridRows({});
    }
    
    onSubmit(form: NgForm){
        if (!this.form.valid)
        return;
    
        this.modelToServer();
        
        (!this.editing ? this.dataService.create(this.serverModel) : this.dataService.change(this.serverModel))
        .then((res: any) => {    
            this.msgService.add({ key: 'tst', severity: res.Code == 0 ? 'success' : 'error', summary: 'Attention!', detail: res.Description });
    
            if (res.Code == 0)
            this.back(true);
        }).catch((error: any) => (this.msgService.add({ key: 'tst', severity: 'error', summary: 'Error!', detail: error.message })));
    }
    
    modelToServer() {
        this.serverModel = {};
    
        var props = Object.keys(this.model);
        for (let index = 0; index < props.length; index++) {
        const prop = props[index];
        
        if (prop == 'Active') {
            this.serverModel.Active = this.model.Active ? "Y" : "N"; 
            continue;
        }
    
        if (this.model[prop] && typeof this.model[prop] == 'object')
            this.serverModel[prop] = this.model[prop][prop];    
        else
            this.serverModel[prop] = this.model[prop];
        }
    }
    
    loadSelects() {
        if (!this.selectsLoaded){
        this.selects.forEach(s => {
            s.load();
        });
        this.selectsLoaded = true;
        }
    }
    
    selectLoaded(select: PsSelectComponent){
        this.selectsLoadedCounter++;
        if (this.mode == 'form' && this.editing){    
            this.model[select.id] = select.dataOptions?.find(d => d[select.id] == this.detail[select.id]);
            select.value = this.model[select.id];
            select.onSelectChange({value: this.model[select.id]});    
        }
    
        this.fillModel();
    }
    
    fillModel() {        
        //after loading all the 'selects' fills the model
        if (this.selects.length == this.selectsLoadedCounter){
        var props = Object.keys(this.detail);
        for (let index = 0; index < props.length; index++) {
            const prop = props[index];
            if (!this.model[prop] || prop == 'Active')
            this.model[prop] = this.detail[prop];
        }      
        }
    }
    
    //##################################
    //########## Form End ##############
    //##################################
}