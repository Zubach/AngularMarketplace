import { Component, OnInit } from '@angular/core';
import { Product } from '../../../../Models/product/product.model';
import { ProductService } from '../../../../services/product.service';

@Component({
  selector: 'app-seller-products-list',
  templateUrl: './seller-products-list.component.html',
  styleUrl: './seller-products-list.component.css'
})
export class SellerProductsListComponent implements OnInit {
  product_list!:Product[];
  constructor(private productService:ProductService){}
  ngOnInit(): void {
    this.productService.getSellerProducts().subscribe({
      next: (res)=>{
       this.product_list = res;
      },
      error: (response)=>{
      }
    })
  }

}
