import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  isLoggedIn:boolean;
  constructor(private http:HttpClient,private authService:AuthService){
    this.isLoggedIn = authService.isLoggedIn();
  }
  
  logout(){
    this.authService.deleteToken();
  }
}
