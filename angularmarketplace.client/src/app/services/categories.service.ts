 import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Category } from '../Models/category/category.model';
import { CreateCategory } from '../Models/category/create-category.model';
import { ConvertService } from './convert.service';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private http:HttpClient,private convert:ConvertService) { }
  baseUrl = environment.apiUrl + '/api/productcategories';
  getMainCategories(){
    return this.http.get<Category[]>(this.baseUrl + "/get_main");
  }
  generateCreateCategoryModel(){
    return this.http.get<CreateCategory>(this.baseUrl + "/generate_category_dto");
  }
  createCategory(createCategoryModel:CreateCategory){
    return this.http.post(this.baseUrl + "/create_category",this.buildFormData(createCategoryModel));
  }
  getAvailableCategories(){
    return this.http.get<Category[]>(this.baseUrl + '/get_available_categories');
  }

  private buildFormData(model:CreateCategory):FormData{
    

    //ignoring parent subCategories and parent
    if(model.parent){
      model.parent.parent = undefined;
      model.parent.subCategoriesList = [];
     
    }

    const formData = this.convert.objectToFormData(model);


    return formData;
  }
}
