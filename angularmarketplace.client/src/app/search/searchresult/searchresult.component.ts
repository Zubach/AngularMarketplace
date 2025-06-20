import { Component, computed, OnInit, TemplateRef, viewChild, ViewChild } from '@angular/core';
import { Product } from '../../Models/product.model';
import { ProductService } from '../../services/product.service';
import { SearchService } from '../../services/search.service';
import { ActivatedRoute } from '@angular/router';
import { Category } from '../../Models/category.model';

@Component({
  selector: 'app-searchresult',
  templateUrl: './searchresult.component.html',
  styleUrl: './searchresult.component.css'
})
export class SearchresultComponent implements OnInit {

  url_title:string;
  mask:string;
  productsList!:Product[];
  categoriesList!:Category[];
  mode: string ='';


constructor(private productService:ProductService,private searchService:SearchService, activeRoute:ActivatedRoute){
  this.url_title = activeRoute.snapshot.params["url_title"];
  this.mask = activeRoute.snapshot.params["mask"];
  
}
  ngOnInit(): void {
    if(this.url_title && this.mask)
        this.searchService.searchByMaskAndUrlTitle(this.url_title,this.mask).subscribe((response)=>{
          switch(response.type){
              case "CategoriesList":
                this.mode = "category_page";
                this.categoriesList = response.data;
                break;
              case "ProductsList":
                this.mode = "products_list";
                this.productsList = response.data;
                break; 
              case "ProductDetails":
                this.mode = "product_details";
                this.productsList = response.data;
                break;
          }
          
        });
   
  }

 
}
