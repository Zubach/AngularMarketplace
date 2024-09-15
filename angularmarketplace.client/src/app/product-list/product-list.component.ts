import { Component, OnInit } from '@angular/core';
import { Product } from '../Models/product.model';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  public products!: Product[];
  constructor(private productService: ProductService){

  }
  ngOnInit(){
    this.productService.getProducts().subscribe((data: Product[])=>{
      this.products = data;
    });
  }
}
