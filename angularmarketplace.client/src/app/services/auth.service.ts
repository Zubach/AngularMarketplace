import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  isLoggedIn(){
    return localStorage.getItem(TOKEN_KEY)!=null ? true : false;
  }

  saveToken(token:string){
    localStorage.setItem(TOKEN_KEY,token);
  }

  deleteToken(){
    localStorage.removeItem(TOKEN_KEY);
  }
}
