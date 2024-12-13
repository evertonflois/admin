import { Component } from '@angular/core';
import { PrivateComponent } from '../../private.component';

@Component({
  selector: 'app-rightpanel',
  templateUrl: './rightpanel.component.html',
  styleUrls: ['./rightpanel.component.css']
})
export class RightpanelComponent {
  constructor(public appMain: PrivateComponent) {}
}
