import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { CategoriesService } from '../../services/categories.service';
import { NgbAccordionDirective, NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';
import { Category } from '../../Models/category/category.model';

@Component({
  selector: 'app-select-category',
  templateUrl: './select-category.component.html',
  standalone:true,
  imports:[NgbAccordionModule],
  styleUrl: './select-category.component.css'
})
export class SelectCategoryComponent {
    @Input() list!: Category[];
    @Output() changeCategoryEvent = new EventEmitter<Category>();
    @Input() parent?:string;
    @ViewChild('selectCategoryAccordion') accordion!:NgbAccordionDirective;

    changeCategory(category:Category){
      
      this.changeCategoryEvent.emit(category);
    }
    
    collapseAll(){
      this.accordion.collapseAll();
    }
    
}
