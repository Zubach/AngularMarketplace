import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../services/product.service';
import { ModerationProduct } from '../../../../Models/product/moderation-product.model';
import { ModeratorProductsListComponent } from './products-list/products-list.component';

@Component({
  standalone: true,
  imports: [ModeratorProductsListComponent],
  selector: 'app-moderator-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ModeratorProductsComponent implements OnInit {
  products:ModerationProduct[] = [];
  constructor(private productService:ProductService){}
  ngOnInit(): void {
    this.productService.getModerationProducts().subscribe({
      next: data =>{
        this.products = data;
      },
      error: response => {

      }
    });
  }

}
