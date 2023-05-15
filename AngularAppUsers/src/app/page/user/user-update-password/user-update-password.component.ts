import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ProfileModel } from 'src/app/model/profile-model';
import { UserModel } from 'src/app/model/user-model';
import { UserUpdatePasswordModel } from 'src/app/model/user-update-password-model';
import { UserService } from 'src/app/service/user.service';
import { ConfirmPasswordValidator } from 'src/app/validation/confirm-password.validator';

@Component({
  selector: 'app-user-update-password',
  templateUrl: './user-update-password.component.html',
  styleUrls: ['./user-update-password.component.css']
})
export class UserUpdatePasswordComponent implements OnInit {
  sending:boolean;      // It is used to enable the save button to avoid the double click
  form:FormGroup;
  profileList:ProfileModel[] = [];
  messageFromAPI:string;
  user:UserModel;
  
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      let userId = +params['id']; // (+) converts string 'id' to a number

      // Waits to the user details
      this.userService.getUser(userId)
                    .subscribe(resp => {
                      this.user = resp;

                      this.form = this.fb.group({
                        UserId: {value: this.user.UserId, disabled: true},
                        LoginName: {value: this.user.LoginName, disabled: true},
                        Password: ['', Validators.compose([Validators.required, Validators.minLength(8)])],
                        PasswordConfirmation: ['', Validators.required],
                      },
                      {
                        validator: ConfirmPasswordValidator("Password", "PasswordConfirmation")
                      });
                    });

   });
  }

  save(): void {
    this.sending = true;

    if(this.form.valid){
      let userToUpdate:UserUpdatePasswordModel = <UserUpdatePasswordModel>this.form.value;
      userToUpdate.UserId = this.user.UserId;
      
      this.userService.saveChangePasswordUser(userToUpdate)
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
