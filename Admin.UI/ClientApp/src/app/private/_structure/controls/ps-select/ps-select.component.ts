import { Component, EventEmitter, Inject, Input, OnInit, Optional, Output, ViewChild } from '@angular/core';
import { NG_ASYNC_VALIDATORS, NG_VALIDATORS, NG_VALUE_ACCESSOR, NgForm, NgModel } from '@angular/forms';
import { ElementBase } from '../element-base';
import { BaseService } from 'src/app/_services/base.service';

@Component({
  selector: 'app-ps-select',
  templateUrl: './ps-select.component.html',
  styleUrl: './ps-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: PsSelectComponent,
      multi: true
    }
  ]
})
export class PsSelectComponent extends ElementBase<any> implements OnInit {
  @Input()
  public form!: NgForm;

  @Input()
  id!: string;

  @Input()
  label!: string;

  @Input()
  optionLabel!: string;

  @Input()
  required = false;

  @Input()
  disabled = false;

  @Input()
  lazy = false;

  @Input()
  showId = true;

  @ViewChild(NgModel)
   model!: NgModel;
  
   @Input()
   dataService!: any;

   @Input()
   params!: any;

   @Input()
   nestedSelects!: PsSelectComponent[];

   @Output()
  SelectChanged = new EventEmitter();

  @Output()
  SelectLoaded = new EventEmitter();

  parentSelect!: PsSelectComponent;
  dataOptions: any[] | undefined;

  public identifier = `ps-select-${identifier++}`;

  constructor( 
    @Optional()
    @Inject(NG_VALIDATORS)
    validators: Array<any>,
    @Optional()
    @Inject(NG_ASYNC_VALIDATORS)
    asyncValidators: Array<any>
    )
  {  
      super(validators, asyncValidators);
  }

  ngOnInit() {
   if (!this.lazy) 
    this.load();

    if (this.nestedSelects)
    {
      for (let index = 0; index < this.nestedSelects.length; index++) {
        this.nestedSelects[index].parentSelect = this;      
      }
    }
      
  }

  load() {
    if (this.parentSelectOk())
    {
      this.dataService.getCombo(this.params).subscribe((res: any) => {
        this.dataOptions = res;
        this.SelectLoaded.emit();      
      });
    }
  }

  onSelectChange(event: any){
    if (event.value == null)
      this.value = undefined;    

      if (this.nestedSelects)
      {        
        if (this.value)
        {
          for (let index = 0; index < this.nestedSelects.length; index++) {
            this.nestedSelects[index].parentSelect = this;      
            this.nestedSelects[index].params = { [this.id] : this.value[this.id] };
            this.nestedSelects[index].load();
          }
        }
        else
        {
          for (let index = 0; index < this.nestedSelects.length; index++) {
            this.nestedSelects[index].value = undefined;
            this.nestedSelects[index].dataOptions = [];            
          }
        }
      }

      this.SelectChanged.emit(event);
  }

  getSelectedValue() {
    return this.value ? 
                      this.showId ? `${this.value[this.id]} - ${this.value[this.optionLabel]}` : `${this.value[this.optionLabel]}`
                      : undefined;
  }

  isDisabled() {
    return this.disabled || !this.parentSelectOk();
  }

  parentSelectOk(){
    return !this.parentSelect || (this.parentSelect && this.parentSelect.value);
  }
}
let identifier = 0;