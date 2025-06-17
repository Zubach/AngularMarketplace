import { Component, computed, OnInit, TemplateRef, viewChild, ViewChild } from '@angular/core';
import { Product } from '../../Models/product.model';
import { ProductService } from '../../services/product.service';
import { SearchService } from '../../services/search.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-searchresult',
  templateUrl: './searchresult.component.html',
  styleUrl: './searchresult.component.css'
})
export class SearchresultComponent implements OnInit {

  url_title:string;
  mask:string;
  productslist!:Product[];
  isProductDetails:boolean = false;


constructor(private productService:ProductService,private searchService:SearchService, activeRoute:ActivatedRoute){
  this.url_title = activeRoute.snapshot.params["url_title"];
  this.mask = activeRoute.snapshot.params["mask"];
  
}
  ngOnInit(): void {
    if(this.url_title && this.mask)
        this.searchService.searchByMaskAndUrlTitle(this.url_title,this.mask).subscribe((data:Product[])=>this.productslist = data);
    if(this.mask[0] == 'p'){
      this.isProductDetails = true;
    }
    else if (this.mask[0] == 'c'){
      this.isProductDetails = false;
    }
    else {
      // product dont found
    }
  }

 
}
