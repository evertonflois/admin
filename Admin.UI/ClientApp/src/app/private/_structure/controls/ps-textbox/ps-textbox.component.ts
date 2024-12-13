import { Component, Inject, Input, Optional, ViewChild } from '@angular/core';
import { NG_ASYNC_VALIDATORS, NG_VALIDATORS, NG_VALUE_ACCESSOR, NgForm, NgModel } from '@angular/forms';
import { ElementBase } from '../element-base';

@Component({
  selector: 'app-ps-textbox',
  templateUrl: './ps-textbox.component.html',
  styleUrl: './ps-textbox.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: PsTextboxComponent,
      multi: true
    }
  ]
})
export class PsTextboxComponent extends ElementBase<string> {  

  @Input()
  public form!: NgForm;

  @Input()
  id!: string;

  @Input()
  label!: string;

  @Input()
  required = false;

  @Input()
  disabled = false;

  @Input()
  placeholder = "";

  @Input()
  maxlength!: number;

  @ViewChild(NgModel, { static: false})
   model!: NgModel;

   public identifier = `ps-textbox-${identifier++}`;

constructor( 
  @Optional()
  @Inject(NG_VALIDATORS)
  validators: Array<any>,
  @Optional()
  @Inject(NG_ASYNC_VALIDATORS)
  asyncValidators: Array<any>)
{  
    super(validators, asyncValidators);
}  

}
let identifier = 0;