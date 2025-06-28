import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';
import { UserRegistration } from '../Models/user-registration.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http:HttpClient) { }
  baseUrl = environment.apiUrl + '/api/account';

  isLoggedIn(){
    return localStorage.getItem(TOKEN_KEY)!=null ? true : false;
  }

  saveToken(token:string){
    localStorage.setItem(TOKEN_KEY,token);
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }
  register(user:UserRegistration){
    return this.http.post<UserRegistration>(this.baseUrl + '/register',user);
  }
}
