import { Component, Input } from '@angular/core';
import { Category } from '../Models/category.model';

@Component({
  selector: 'app-categorypage',
  templateUrl: './categorypage.component.html',
  styleUrl: './categorypage.component.css'
})
export class CategorypageComponent {
  @Input() categoriesList!:Category[];
}
