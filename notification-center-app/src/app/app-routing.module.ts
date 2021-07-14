import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {NotificationsComponent} from './notifications/notifications.component';


const routes: Routes = [
  { path: '', redirectTo: '/notifications', pathMatch: 'full'},
  { path: 'notifications', component: NotificationsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
