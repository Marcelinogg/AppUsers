import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { ProfileModel } from 'src/app/model/profile-model';
import { UserModel } from 'src/app/model/user-model';
import { UserUpdateModel } from 'src/app/model/user-update-model';
import { ProfileService } from 'src/app/service/profile.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.css']
})
export class UserUpdateComponent implements OnInit {
  sending:boolean;      // It is used to enable the save button to avoid the double click
  form:FormGroup;
  profileList:ProfileModel[] = [];
  messageFromAPI:string;
  user:UserModel;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      let userId = +params['id']; // (+) converts string 'id' to a number

      // Waits to the both requests
      forkJoin([
        this.profileService.getProfiles(),
        this.userService.getUser(userId),
      ]).subscribe(resp => {
        this.profileList = resp[0];
        this.user = resp[1];
  
        this.form = this.fb.group({
          UserId: {value: this.user.UserId, disabled: true},
          LoginName: [this.user.LoginName, Validators.required],
          FullName: [this.user.FullName, Validators.required],
          Email: [this.user.Email, Validators.email],
          Avatar: [this.user.Avatar],
          ProfileId: [this.user.ProfileId, Validators.required],
        });
      });

   });
  }

  save(): void {
    this.sending = true;

    if(this.form.valid){
      let userToUpdate:UserUpdateModel = <UserUpdateModel>this.form.value;
      userToUpdate.UserId = this.user.UserId;

      this.userService.saveChangeDataUser(userToUpdate)
                      .subscribe(resp => {
                        this.router.navigate([""]);   // Go to home to see the updated item
                      },
                      error => {
                        console.log(error);
                        this.sending = false;

                        if(error.status == 400){      // Badrequest is a error known
                          this.messageFromAPI = error?.error?.Message;
                        }
                        else {
                          this.router.navigate(["error"]);
                        }
                      }
                      );
    }
    else {
      this.sending = false;
      this.form.markAllAsTouched();
    }
  }

}
