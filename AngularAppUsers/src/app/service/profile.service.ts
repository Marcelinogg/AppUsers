import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProfileModel } from '../model/profile-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(
    private http: HttpClient
  ) { }

  getProfiles(): Observable<ProfileModel[]> {
    return this.http.get<ProfileModel[]>(`${environment.api}/profiles`);
  }
}
