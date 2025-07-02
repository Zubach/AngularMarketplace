import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Category } from '../Models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private http:HttpClient) { }
  baseUrl = environment.apiUrl + '/api/productcategories';
  getMainCategories(){
    return this.http.get<Category[]>(this.baseUrl + "/get_main");
  }
}
