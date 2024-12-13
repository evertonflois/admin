import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-ps-toolbar',
  templateUrl: './ps-toolbar.component.html',
  styleUrl: './ps-toolbar.component.css'
})
export class PsToolbarComponent {

    @Input()
    title!: string;

    @Input()
    can!: any;

    @Output()
    NewClick = new EventEmitter();

    @Output()
    EditSelectedRowClick = new EventEmitter();

    @Output()
    SaveClick = new EventEmitter();

    @Output()
    CleanClick = new EventEmitter();

    @Output()
    RemoveClick = new EventEmitter();

    @Output()
    BackClick = new EventEmitter();    

    new() {
      this.NewClick.emit();
    }

    editSelectedRow() {
      this.EditSelectedRowClick.emit();
    }

    save(event: any) {
      this.SaveClick.emit(event);
    }

    clean() {
      this.CleanClick.emit();
    }

    remove(event: any) {
      this.RemoveClick.emit(event);
    }

    back() {
      this.BackClick.emit();
    }

    setCan(can: any){
      this.can = can;
    }

}
