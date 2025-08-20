import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../Models/product/product.model';
import { environment } from '../../environments/environment';
import { CreateProduct } from '../Models/product/create-product.model';
import { ConvertService } from './convert.service';
import { ModerationProduct } from '../Models/product/moderation-product.model';


@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http:HttpClient,private convert:ConvertService) { }
  baseUrl = environment.apiUrl + '/api/products/';
  
  getProducts(){
    return this.http.get<Product[]>(this.baseUrl);
  }
  getProduct(id:number){
    return this.http.get<Product>(this.baseUrl + id.toString());
  }
  getProductsByCategory(id:number){
    return this.http.get<Product[]>(this.baseUrl + 'category/' + id.toString());
  }

  getSellerProducts(){
    return this.http.get<Product[]>(this.baseUrl + 'seller');
  }

  createProduct(product:CreateProduct){
    const formData = this.convert.objectToFormData(product);
    return this.http.post(this.baseUrl ,formData);
  }

  getModerationProducts(){
    return this.http.get<ModerationProduct[]>(this.baseUrl + 'moderation')
  }
}
