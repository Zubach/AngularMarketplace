import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaderResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Wishlist } from '../Models/User/wishlist.model';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {

  baseUrl = environment.apiUrl + '/api/wishlist';
  constructor(private http:HttpClient, private authService:AuthService) { }

  getUserWishlists(){
    return this.http.get<Wishlist[]>(this.baseUrl + "/get_user_wishlists");
  }
}
