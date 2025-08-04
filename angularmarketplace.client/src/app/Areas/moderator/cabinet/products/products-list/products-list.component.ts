import { Component, Input } from '@angular/core';
import { ModerationProduct } from '../../../../../Models/product/moderation-product.model';
import { ProductVisibilityStatus } from '../../../../../enums/product-visibility-status';

@Component({
  selector: 'app-moderator-products-list',
  templateUrl: './products-list.component.html',
  styleUrl: './products-list.component.css',
  standalone: true
})
export class ModeratorProductsListComponent {
  @Input() products!:ModerationProduct[];
  public StatusEnum = ProductVisibilityStatus;
}
