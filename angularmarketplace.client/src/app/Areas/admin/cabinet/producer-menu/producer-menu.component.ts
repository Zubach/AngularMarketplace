import { Component, OnInit, ViewChild } from '@angular/core';
import { ProducerService } from '../../../../services/producer.service';
import { Producer } from '../../../../Models/producer/producer.model';
import { ProducerTableComponent } from './producer-table/producer-table.component';
import { CategoriesService } from '../../../../services/categories.service';
import { Category } from '../../../../Models/category/category.model';
import { AddCategoryComponent } from '../categories-menu/add-category/add-category.component';
import { AddProducerComponent } from './add-producer/add-producer.component';

@Component({
  selector: 'app-producer-menu',
  templateUrl: './producer-menu.component.html',
  styleUrl: './producer-menu.component.css',
  standalone: true,
  imports: [ProducerTableComponent,AddProducerComponent]
})
export class ProducerMenuComponent implements OnInit {
  producers!:Producer[];
  categories!:Category[];
  
  @ViewChild(AddProducerComponent) addProducerForm!:AddProducerComponent;

  constructor(private producerService:ProducerService,private categoriesService:CategoriesService){}

  ngOnInit(): void {  

    this.loadProducers();
     this.categoriesService.getMainCategories().subscribe((data:Category[])=>{this.categories = data});

  }

  addProducerShowModal(){
    this.addProducerForm.showModalForm();
    console.log(this.categories);
  }
  onProducerCreated(response:Object){
    this.loadProducers(); // update producers table
  }

  loadProducers(){
   this.producerService.getProducers().subscribe((data:Producer[])=>{this.producers = data});
  }
}
