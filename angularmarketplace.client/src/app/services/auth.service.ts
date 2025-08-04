import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';
import { UserRegistration } from '../Models/User/user-registration.model';
import { environment } from '../../environments/environment';
import { UserLogin } from '../Models/User/user-login';
import {jwtDecode} from 'jwt-decode'

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http:HttpClient) { }
  baseUrl = environment.apiUrl + '/api/account';

  isLoggedIn(){
    return !this.isTokenExpired (this.getToken());
  }

  saveToken(token:string){
    localStorage.setItem(TOKEN_KEY,token);
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }

  getToken():string{
     return localStorage.getItem(TOKEN_KEY) ?? '';
  }
  register(user:UserRegistration){
    return this.http.post<UserRegistration>(this.baseUrl + '/register',user);
  }
  login(user:UserLogin){
    return this.http.post<UserLogin>(this.baseUrl + '/login',user);
  }
  isTokenExpired(token:string):boolean{
    if(token && token !== ''){
      try{
        const decodedToken:any = jwtDecode(token);
        const expirationTime = decodedToken.exp * 1000; 
        return Date.now() >= expirationTime;
      }
      catch(ex){
          return true;
      }
    }
    return true;
  }
  isAdmin():boolean{
    const token = this.getToken();
    if(token && token !==''){
      const decodedToken:any = jwtDecode(token);
      console.log(decodedToken);
      return decodedToken.role == 'Admin';
    }
    return false;
  }
  isSeller():boolean{
    const token = this.getToken();
    if(token && token !==''){
      const decodedToken:any = jwtDecode(token);
      
      return decodedToken.role == 'Seller';
    }
    return false;
  }
  getUserRole():string{
    const token = this.getToken();
    if(token && token != ""){
      const decodedToken:any = jwtDecode(token);
      return decodedToken.role;
    }
    return "";
  }
}
