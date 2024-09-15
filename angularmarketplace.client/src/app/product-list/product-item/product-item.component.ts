import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../Models/product.model';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.css'
})
export class ProductItemComponent implements OnInit {
  @Input()
  product!: Product;
  constructor(){}
  ngOnInit(){
    
  }

}
