import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent implements OnInit {
  @Input() form:FormGroup;

  constructor() { }

  ngOnInit(): void {
  }

  onFileChange(event:any) {
    const reader = new FileReader();
    
    if(event.target.files && event.target.files.length) {
      
      const [file] = event.target.files;
      reader.readAsDataURL(file);
    
      reader.onload = () => {
     
        this.form.patchValue({
          Avatar: reader.result as string
        });
   
      };   
    }
  }

}
