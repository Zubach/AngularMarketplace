import { Component, OnInit } from '@angular/core';
import { Category } from '../Models/category/category.model';
import { CategoriesService } from '../services/categories.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-mainpage',
  templateUrl: './mainpage.component.html',
  styleUrl: './mainpage.component.css'
})
export class MainpageComponent implements OnInit{
  categoriesList!:Category[];
  constructor(private categoriesService:CategoriesService,activatedRoutes:ActivatedRoute){}

  ngOnInit(): void {
   this.categoriesService.getMainCategories().subscribe((data:Category[])=> this.categoriesList = data);
   
  }

}
