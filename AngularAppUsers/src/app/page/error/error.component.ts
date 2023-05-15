import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {
  image:string = "/assets/engineer_at_work.jpg";
  
  constructor() { }

  ngOnInit(): void {
  }

}
