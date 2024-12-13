import { Component, Inject, Input, Optional, ViewChild } from '@angular/core';
import { ElementBase } from '../element-base';
import { NG_ASYNC_VALIDATORS, NG_VALIDATORS, NG_VALUE_ACCESSOR, NgForm, NgModel } from '@angular/forms';

@Component({
  selector: 'app-ps-active',
  templateUrl: './ps-active.component.html',
  styleUrl: './ps-active.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: PsActiveComponent,
      multi: true
    }
  ]
})
export class PsActiveComponent extends ElementBase<any> {
  
  @Input()
  public form!: NgForm;

  @Input()
  id!: string;

  @Input()
  disabled!: boolean;
  
  @ViewChild(NgModel)
  model!: NgModel;

  public identifier = `ps-active-${identifier++}`;

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
}
let identifier = 0;