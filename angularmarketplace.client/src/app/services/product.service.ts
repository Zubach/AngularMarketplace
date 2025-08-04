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
  baseUrl = environment.apiUrl + '/api/products';
  
  getProducts(){
    return this.http.get<Product[]>(this.baseUrl + '/get_products');
  }
  getProduct(id:number){
    return this.http.get<Product>(this.baseUrl + '/get_product_details/'+ id.toString());
  }
  getProductsByCategory(id:number){
    return this.http.get<Product[]>(this.baseUrl + '/get_products_by_category/' + id.toString());
  }

  getSellerProducts(){
    return this.http.get<Product[]>(this.baseUrl + '/get_seller_products');
  }

  createProduct(product:CreateProduct){
    const formData = this.convert.objectToFormData(product);
    return this.http.post(this.baseUrl + '/create_product',formData);
  }

  getModerationProducts(){
    return this.http.get<ModerationProduct[]>(this.baseUrl + '/get_moderation_products')
  }
}
