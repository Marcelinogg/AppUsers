import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { UserModel } from '../model/user-model';
import { UserNewModel } from '../model/user-new-model';
import { UserUpdateModel } from '../model/user-update-model';
import { UserUpdatePasswordModel } from '../model/user-update-password-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

  getUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(`${environment.api}/users`);
  }

  getUser(userId:number): Observable<UserModel> {
    return this.http.get<UserModel>(`${environment.api}/users/${userId}`);
  }

  saveNewUser(user:UserNewModel): Observable<UserModel> {
    return this.http.post<UserModel>(`${environment.api}/users`, user);
  }
  
  saveChangeDataUser(user:UserUpdateModel): Observable<UserModel> {
    return this.http.put<UserModel>(`${environment.api}/users`, user);
  }

  saveChangePasswordUser(user:UserUpdatePasswordModel): Observable<UserModel> {
    return this.http.put<UserModel>(`${environment.api}/users/changepassword`, user);
  }

  removeUser(userId:number): Observable<UserModel[]>{
    return this.http.delete<UserModel[]>(`${environment.api}/users/${userId}`);
  }
}
