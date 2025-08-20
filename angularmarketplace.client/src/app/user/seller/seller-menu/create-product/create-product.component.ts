import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { AbstractControl, Form, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { NgbDropdownModule, NgbModal, NgbTypeaheadModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductService } from '../../../../services/product.service';
import { AuthService } from '../../../../services/auth.service';
import { CreateProduct } from '../../../../Models/product/create-product.model';
import { CategoriesService } from '../../../../services/categories.service';
import { Category } from '../../../../Models/category/category.model';
import { SelectCategoryComponent } from "../../../../categories-list/select-category/select-category.component";
import { MultipleDragNDropFileInputComponent } from '../../../../dnd-file-input/multiple-dnd-file-input/multiple-dnd-file-input.component';
import { Producer } from '../../../../Models/producer/producer.model';
import { debounceTime, map, Observable, OperatorFunction } from 'rxjs';
import { ProducerService } from '../../../../services/producer.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  standalone:true,
  imports: [FormsModule,ReactiveFormsModule, NgbDropdownModule, SelectCategoryComponent, MultipleDragNDropFileInputComponent, NgbModule,NgbTypeaheadModule],
  styleUrl: './create-product.component.css'
})
export class CreateProductComponent {
  private modalService = inject(NgbModal);
  @ViewChild('content') modalFormComponent!:ElementRef
  form:FormGroup;
  isSubmitted:boolean = false;
  categoriesList!:Category[];
  producersList?:Producer[];
  producer?:Producer;
  constructor(
    private formBuilder:FormBuilder,
    private productService:ProductService,
     private authService:AuthService,
     private categoriesService:CategoriesService,
     private producerService:ProducerService
    ){
    this.form = this.formBuilder.group({
      title:['',Validators.required],
      description:['',Validators.required],
      price:[null,[Validators.required,Validators.min(0)]],
      producer: new FormControl<null | Producer>(null,{validators:[Validators.required,this.producerValidator]}),
      imgs:new FormControl<null | FileList>(null,{validators: [this.imgsValidator]}),
      choosenCategory: new FormControl<null | Category>(null,{validators:[Validators.required]})
    });
  }

  imgsValidator:ValidatorFn = (control:AbstractControl):ValidationErrors | null=>{
    const imgs = control.value as FileList;
    if(!imgs)
      return {required:true};
    if(imgs.length > 6)
      return {max:true};
    return null;
  };
  producerValidator:ValidatorFn = (control:AbstractControl):ValidationErrors | null=>{
    const producer = control.value as Producer;
    if(!(producer 
      && this.producersList
      && this.producersList.some(p => p.id === producer.id))
    ){
      return {categoryRequired: true};
    }
    return null;
  };
  showModalForm(){
    this.form.reset();
    this.isSubmitted = false;
    this.modalService.open(this.modalFormComponent,{centered:true,scrollable:true});
    
    if( this.authService.isSeller()){

      if(this.categoriesList == null){
         this.categoriesService.getMainCategories().subscribe((data:Category[])=>{
          this.categoriesList = data
        });
        
      }
      
    }

  }
  onFilesChange(files:FileList){
    this.form.patchValue({imgs:files});
  }
  onSubmit(){
    this.isSubmitted = true;
    console.log(this.form.controls);
    if(this.form.valid){
      let model:CreateProduct = {
        title: this.form.controls['title'].value,
        description: this.form.controls['description'].value,
        price: this.form.controls['price'].value,
        producerId: (this.form.controls['producer'].value as Producer).id  ,
        categoryMask: (this.form.controls['choosenCategory'].value as Category).mask,
        imgs: this.form.controls['imgs'].value as FileList
      };
      if(model){
        
        console.log(model);

        this.productService.createProduct(model).subscribe({
          next: model =>{

          },
          error: response =>{

          }
        });
      }
      
    }
  }
  onChangeCategory(category:Category){
    if(category.isSubCategory && ( !category.subCategoriesList || category.subCategoriesList?.length == 0)){
      this.form.patchValue({choosenCategory:category});
      this.producerService.getCategoryProducersById(category.id).subscribe((data:Producer[])=>{this.producersList = data;});
    }
  }
  onProducerChange(event:Event){
    const producer = (event.target as HTMLInputElement).value;
    if(producer && this.producersList){
      this.producer = this.producersList.find(p => p.name === producer) || undefined;
      this.form.patchValue({producer: this.producer});
    }
    else
      this.form.patchValue({producer: null});
  }
  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }
  searchProducer: OperatorFunction<string, readonly Producer[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      map((term) =>
        term === ''
          ? this.producersList?.slice(0, 10) ?? []
          : this.producersList?.filter((s) => s.name.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10) ?? []
      )
    );
  formatter = (x: Producer) => x.name;
}
