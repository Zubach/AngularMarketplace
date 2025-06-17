import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from '../../environments/environment.development';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  cdnProvider:string;
  constructor(private http:HttpClient){
    this.cdnProvider = environment.apiUrl.concat("/cdn");
  }
  
}
