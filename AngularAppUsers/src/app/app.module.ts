import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './page/home/home.component';
import { NotFoundComponent } from './page/not-found/not-found.component';
import { UserListComponent } from './page/user/user-list/user-list.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UserUpdateComponent } from './page/user/user-update/user-update.component';
import { UserNewComponent } from './page/user/user-new/user-new.component';
import { UserUpdatePasswordComponent } from './page/user/user-update-password/user-update-password.component';
import { ErrorComponent } from './page/error/error.component';
import { ImageUploadComponent } from './page/image-upload/image-upload.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NotFoundComponent,
    UserListComponent,
    UserUpdateComponent,
    UserNewComponent,
    UserUpdatePasswordComponent,
    ErrorComponent,
    ImageUploadComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
