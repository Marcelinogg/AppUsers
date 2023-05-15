import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ProfileModel } from 'src/app/model/profile-model';
import { UserNewModel } from 'src/app/model/user-new-model';
import { ProfileService } from 'src/app/service/profile.service';
import { UserService } from 'src/app/service/user.service';
import { ConfirmPasswordValidator } from 'src/app/validation/confirm-password.validator';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styleUrls: ['./user-new.component.css']
})
export class UserNewComponent implements OnInit {
  sending:boolean;
  form:FormGroup;
  profileList:ProfileModel[] = [];
  messageFromAPI:string;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private userService: UserService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.profileService.getProfiles()
                    .subscribe(resp => {
                      this.profileList = resp;
                      this.form = this.fb.group({
                        LoginName: ['', Validators.required],
                        FullName: ['', Validators.required],
                        Password: ['', Validators.compose([Validators.required, Validators.minLength(8)])],
                        PasswordConfirmation: ['', Validators.required],
                        Email: ['', Validators.email],
                        Avatar: [''],
                        ProfileId: ['', Validators.required],
                      },
                      {
                        validator: ConfirmPasswordValidator("Password", "PasswordConfirmation")
                      });
                    });
  }

  save(): void {
    this.sending = true;
    console.log(this.form.value);

    // if(this.form.valid){
      if(true){
      this.userService.saveNewUser(<UserNewModel>this.form.value)
                      .subscribe(resp => {
                        this.router.navigate([""]);
                      },
                      error => {
                        console.log(error);
                        this.sending = false;

                        if(error.status == 400){  // Badrequest is a error known
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
