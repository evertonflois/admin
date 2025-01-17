import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { PrivateComponent } from '../../private.component';
import { MenuAppService } from 'src/app/_services/menu-app.service';

@Component({
    /* tslint:disable:component-selector */
    selector: '[app-menuitem]',
    /* tslint:enable:component-selector */
    template: `
    <ng-container>
        <a [attr.href]="item.Url" (click)="itemClick($event)" *ngIf="(!item.RouterLink || item.Items) && item.Visible !== false"
        (mouseenter)="onMouseEnter()" (keydown.enter)="itemClick($event)"
        [attr.target]="item.target" [attr.tabindex]="0" [ngClass]="item.Class" pRipple>
            <i class="layout-menuitem-icon" [ngClass]="item.Icon"></i>
            <span>{{item.Label}}</span>
            <span class="menuitem-badge" [ngClass]="item.badgeStyleClass" *ngIf="item.badge">{{item.badge}}</span>
            <i class="pi pi-fw pi-angle-down layout-menuitem-toggler" *ngIf="item.Items"></i>
        </a>
        <a (click)="itemClick($event)" (mouseenter)="onMouseEnter()" *ngIf="(item.RouterLink && !item.Items) && item.Visible !== false"
        [routerLink]="item.RouterLink" routerLinkActive="active-menuitem-routerlink"
        [routerLinkActiveOptions]="{exact: !item.preventExact}" [attr.target]="item.target" [attr.tabindex]="0" [ngClass]="item.Class" pRipple>
            <i class="layout-menuitem-icon" [ngClass]="item.Icon"></i>
            <span>{{item.Label}}</span>
            <span class="menuitem-badge" [ngClass]="item.badgeStyleClass" *ngIf="item.badge">{{item.badge}}</span>
            <i class="pi pi-fw pi-angle-down layout-menuitem-toggler" *ngIf="item.Items"></i>
        </a>
        <div class="submenu-arrow" *ngIf="item.Items && item.Visible !== false"></div>
        <ul *ngIf="(item.Items && active) && item.Visible !== false"
            [@children]="((app.isSlim()||app.isHorizontal()) && root) ? (active ? 'visible' : 'hidden') :
            (active ? 'visibleAnimated' : 'hiddenAnimated')">
            <ng-template ngFor let-child let-i="index" [ngForOf]="item.Items">
                <li app-menuitem [item]="child" [index]="i" [parentKey]="key" [class]="child.badgeClass"></li>
            </ng-template>
        </ul>
    </ng-container>
    `,
    host: {
        '[class.active-menuitem]': 'active'
    },
    animations: [
        trigger('children', [
            state('void', style({
                height: '0px',
                opacity: 0
            })),
            state('hiddenAnimated', style({
                height: '0px',
                opacity: 0
            })),
            state('visibleAnimated', style({
                height: '*',
                opacity: 1
            })),
            state('visible', style({
                height: '*',
                'z-index': 100,
                opacity: 1
            })),
            state('hidden', style({
                height: '0px',
                'z-index': '*',
                opacity: 0
            })),
            transition('visibleAnimated => hiddenAnimated', animate('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
            transition('hiddenAnimated => visibleAnimated', animate('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
            transition('void => visibleAnimated, visibleAnimated => void',
                animate('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
        ])
    ]
})
export class MenuitemComponent implements OnInit, OnDestroy {

    @Input() item: any;

    @Input() index?: number;

    @Input() root?: boolean;

    @Input() parentKey?: string;

    active = false;

    menuSourceSubscription: Subscription;

    menuResetSubscription: Subscription;

    key: string = "";

    constructor(public app: PrivateComponent, public router: Router, private cd: ChangeDetectorRef, private menuService: MenuAppService) {
        this.menuSourceSubscription = this.menuService.menuSource$.subscribe((key?: string) => {
            // deactivate current active menu
            if (this.active && this.key !== key && key?.indexOf(this.key) !== 0) {
                this.active = false;
            }
        });

        this.menuResetSubscription = this.menuService.resetSource$.subscribe(() => {
            this.active = false;
        });

        this.router.events.pipe(filter(event => event instanceof NavigationEnd))
            .subscribe(params => {
                if (this.app.isSlim() || this.app.isHorizontal()) {
                    this.active = false;
                } else {
                    if (this.item.RouterLink) {
                        this.updateActiveStateFromRoute();
                    } else {
                        this.active = false;
                    }
                }
            });
    }

    ngOnInit() {
        if (!(this.app.isSlim() || this.app.isHorizontal()) && this.item.RouterLink) {
            this.updateActiveStateFromRoute();
        }

        this.key = this.parentKey ? this.parentKey + '-' + this.index : String(this.index);
    }

    updateActiveStateFromRoute() {
        this.active = this.router.isActive(this.item.RouterLink[0], !this.item.Items && !this.item.preventExact);
    }

    itemClick(event: Event) {
        // avoid processing disabled items
        if (this.item.disabled) {
            event.preventDefault();
            return;
        }

        // navigate with hover in horizontal mode
        if (this.root) {
            this.app.menuHoverActive = !this.app.menuHoverActive;
        }

        // notify other items
        this.menuService.onMenuStateChange(this.key);

        // execute command
        if (this.item.command) {
            this.item.command({originalEvent: event, item: this.item});
        }

        // toggle active state
        if (this.item.Items) {
            this.active = !this.active;
        } else {
            // activate item
            this.active = true;

            // reset horizontal menu
            if (this.app.isSlim() || this.app.isHorizontal()) {
                this.menuService.reset();
            }

            this.app.overlayMenuActive = false;
            this.app.staticMenuMobileActive = false;
            this.app.menuHoverActive = !this.app.menuHoverActive;
        }
    }

    onMouseEnter() {
        // activate item on hover
        if (this.root && this.app.menuHoverActive && (this.app.isHorizontal() || this.app.isSlim()) && this.app.isDesktop()) {
            this.menuService.onMenuStateChange(this.key);
            this.active = true;
        }
    }

    ngOnDestroy() {
        if (this.menuSourceSubscription) {
            this.menuSourceSubscription.unsubscribe();
        }

        if (this.menuResetSubscription) {
            this.menuResetSubscription.unsubscribe();
        }
    }
}
