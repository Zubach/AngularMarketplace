import { Component, ViewChild } from '@angular/core';
import { CreateProductComponent } from './create-product/create-product.component';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-seller-menu',
  templateUrl: './seller-menu.component.html',
  styleUrl: './seller-menu.component.css'
})
export class SellerMenuComponent {
  @ViewChild(CreateProductComponent) createProductForm!: CreateProductComponent;

  showCreateProductModal(){
    this.createProductForm.showModalForm();
  }
}
