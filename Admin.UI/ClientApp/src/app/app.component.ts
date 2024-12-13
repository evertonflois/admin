import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  theme = 'noir';
  layoutMode = 'static';
  megaMenuMode = 'gradient';
  menuMode = 'light';
  profileMode = 'top';
  inputStyle = 'outlined';
  ripple: boolean = false;
  routerSubscription: any;
  codAssinante = 0;

  constructor(
    private primengConfig: PrimeNGConfig,
    private router: Router,
    private route: ActivatedRoute) {
    this.route.params.subscribe(
      (params: Params) => {
        this.codAssinante = params.codAssinante;
      }
    );
  }

  ngOnInit() {
    this.primengConfig.ripple = true;
    this.ripple = true;    
  }

  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }
}
