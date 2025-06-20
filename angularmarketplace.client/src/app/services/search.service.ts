import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Product } from '../Models/product.model';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http:HttpClient) { }
  baseUrl = environment.apiUrl+ '/api/search' ;
  searchByMaskAndUrlTitle(url_title:string,mask:string){
    return this.http.get<any>(this.baseUrl + '/searchby_urltitle_mask/' + url_title + ';' + mask);
  }
}
