import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './page/home/home.component';
import { NotFoundComponent } from './page/not-found/not-found.component';
import { UserUpdateComponent } from './page/user/user-update/user-update.component';
import { UserNewComponent } from './page/user/user-new/user-new.component';
import { UserUpdatePasswordComponent } from './page/user/user-update-password/user-update-password.component';
import { ErrorComponent } from './page/error/error.component';

const routes: Routes = [
  { path : 'home', component : HomeComponent },
  { path : 'user-new', component : UserNewComponent },
  { path : 'user-update/:id', component : UserUpdateComponent },
  { path : 'user-update-password/:id', component : UserUpdatePasswordComponent },
  { path : 'error', component : ErrorComponent },
  { path : '', redirectTo : '/home', pathMatch: 'full' },
  { path : '**', component : NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
