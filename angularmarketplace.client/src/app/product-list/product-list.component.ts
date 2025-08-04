import { Component, Input, OnInit} from '@angular/core';
import { Product } from '../Models/product/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  @Input()
  public products!: Product[];
  constructor(){

  }
}
