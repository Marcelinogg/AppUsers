import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserModel } from 'src/app/model/user-model';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  userlist:UserModel[]= [];
  
  constructor(
    private userService: UserService,
    private route: Router,
  ) { }

  ngOnInit(): void {
    this.userService.getUsers()
                  .subscribe(resp => this.userlist = resp);
  }

  gotoUserNewPage(): void {
    this.route.navigate(['user-new']);
  }

  gotoUserUpdate(userId:number): void {
    this.route.navigate(['user-update', userId]);
  }

  gotoUserUpdatePassword(userId:number): void {
    this.route.navigate(['user-update-password', userId]);
  }

  removeUser(userId:number) {
    if(confirm(`¿Quitar el usuario ${userId}?`)) {
      this.userService.removeUser(userId)
                    .subscribe(resp => this.userlist = resp);
    }
  }
}
