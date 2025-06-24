import { Component, OnDestroy, OnInit, TemplateRef, viewChild, ViewChild } from '@angular/core';
import { Product } from '../../Models/product.model';
import { ProductService } from '../../services/product.service';
import { SearchService } from '../../services/search.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Category } from '../../Models/category.model';
import { filter, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-searchresult',
  templateUrl: './searchresult.component.html',
  styleUrl: './searchresult.component.css'
})
export class SearchresultComponent implements OnInit, OnDestroy {

  url_title:string | null;
  mask:string | null;
  productsList!:Product[];
  categoriesList!:Category[];
  mode: string ='';
  private unsubscribe$ = new Subject<void>();


constructor(private productService:ProductService,private searchService:SearchService, private activeRoute:ActivatedRoute, private router:Router){
  this.url_title = activeRoute.snapshot.params["url_title"];
  this.mask = activeRoute.snapshot.params["mask"];
}
  ngOnInit(): void {
    this.router.events.pipe(
          filter((event): event is NavigationEnd => event instanceof NavigationEnd),
          takeUntil(this.unsubscribe$)
        ).subscribe((event: NavigationEnd) => {
          // this block required for refresh component when updating params 
          this.activeRoute.paramMap.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
            this.url_title = params.get("url_title");
            this.mask = params.get("mask");
            this.search();
          });

        });
    this.search();
   
  }
  ngOnDestroy(): void {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
  }
  search(){
    
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
