import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { PrivateComponent } from '../../private.component';
import { BreadcrumbService } from 'src/app/_services/breadcrumb.service';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent {
  subscription: Subscription;
  items?: MenuItem[];
  totNotify?: number;
  home: MenuItem | undefined;

  constructor(
      public breadcrumbService: BreadcrumbService,
      public appMain: PrivateComponent) {
      this.subscription = breadcrumbService.itemsHandler.subscribe(response => {
          this.items = response;
      });
  }

  ngOnInit() {
      this.totNotify = 3;
      this.home = { icon: 'pi pi-home', routerLink: '/' };
  }

  ngOnDestroy() {
      if (this.subscription) {
          this.subscription.unsubscribe();
      }
  }
}
