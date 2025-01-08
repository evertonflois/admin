import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PrivateComponent } from './private.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from '../_helpers/guard/auth-guard';
import { ProfileComponent } from './authorization/profile/profile.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { UserComponent } from './authorization/user/user.component';

const routes: Routes = [
  {
    path: '',
    component: PrivateComponent,
    children: [
      {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'access-denied',
        component: AccessDeniedComponent,
        canActivate: [AuthGuard],
      },      
      {
        path: 'authorization/user',
        component: UserComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'authorization/profile',
        component: ProfileComponent,
        canActivate: [AuthGuard],
      },

      // // Pages
      // {
      //   path: 'pages',
      //   loadChildren: () =>
      //     import('./pages/pages.module').then(
      //       (m) => m.PagesModule
      //     ),
      // },

      { path: '*', redirectTo: '/login' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrivateRoutingModule { }
