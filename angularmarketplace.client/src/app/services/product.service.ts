import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../Models/product.model';
import { environment } from '../../environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http:HttpClient) { }
  baseUrl = environment.apiUrl + '/api/products';
  
  getProducts(){
    return this.http.get<Product[]>(this.baseUrl + '/get_products');
  }
}
