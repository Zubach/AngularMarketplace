import { Component, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from '../../../../services/auth.service';
import { CategoriesService } from '../../../../services/categories.service';
import { AddCategoryComponent } from './add-category/add-category.component';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-categories-menu',
  templateUrl: './categories-menu.component.html',
  styleUrl: './categories-menu.component.css'
})
export class CategoriesMenuComponent {
  @ViewChild(AddCategoryComponent) addCategoryComponent!:AddCategoryComponent;
  constructor(private authService:AuthService,private categoriesService:CategoriesService,private http:HttpClient){}

  showAddCategoryModal(){
    this.addCategoryComponent.showModalForm();
  }
  
}
