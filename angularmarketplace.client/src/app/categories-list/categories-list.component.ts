import { Component, Input } from '@angular/core';
import { Category } from '../Models/category/category.model';

@Component({
  selector: 'app-categories-list',
  templateUrl: './categories-list.component.html',
  styleUrl: './categories-list.component.css'
})
export class CategoriesListComponent {
  @Input()
  public categories!:Category[];
  constructor(){
    
  }

}
