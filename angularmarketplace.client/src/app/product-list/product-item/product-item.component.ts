import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../Models/product/product.model';
import { ProductService } from '../../services/product.service';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.css'
})
export class ProductItemComponent implements OnInit {
  @Input()
  product!: Product;
  cdnProvider:string;
  constructor(){
    this.cdnProvider = environment.cdnUrl;
  }
  ngOnInit(){
    
  }

}
