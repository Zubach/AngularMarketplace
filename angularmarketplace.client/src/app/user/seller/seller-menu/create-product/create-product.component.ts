import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { AbstractControl, Form, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { NgbDropdownModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductService } from '../../../../services/product.service';
import { AuthService } from '../../../../services/auth.service';
import { CreateProduct } from '../../../../Models/product/create-product.model';
import { CategoriesService } from '../../../../services/categories.service';
import { Category } from '../../../../Models/category/category.model';
import { SelectCategoryComponent } from "../../../../categories-list/select-category/select-category.component";
import { MultipleDragNDropFileInputComponent } from '../../../../dnd-file-input/multiple-dnd-file-input/multiple-dnd-file-input.component';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  standalone:true,
  imports: [ReactiveFormsModule, NgbDropdownModule, SelectCategoryComponent,MultipleDragNDropFileInputComponent],
  styleUrl: './create-product.component.css'
})
export class CreateProductComponent {
  private modalService = inject(NgbModal);
  @ViewChild('content') modalFormComponent!:ElementRef
  form:FormGroup;
  isSubmitted:boolean = false;
  categoriesList!:Category[];
  //choosenCategory?:Category;
  constructor(private formBuilder:FormBuilder,private productService:ProductService, private authService:AuthService,private categoriesService:CategoriesService){
    this.form = this.formBuilder.group({
      title:['',Validators.required],
      description:['',Validators.required],
      price:[null,[Validators.required,Validators.min(0)]],
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
  showModalForm(){
    this.form.reset();
    this.isSubmitted = false;
    this.modalService.open(this.modalFormComponent,{centered:true,scrollable:true});
    
    if( this.authService.isSeller()){

      if(this.categoriesList == null){
         this.categoriesService.getMainCategories().subscribe((data:Category[])=>{
           console.log(data);
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
    if(this.form.valid){
      let model:CreateProduct = {
        title: this.form.controls['title'].value,
        description: this.form.controls['description'].value,
        price: this.form.controls['price'].value,
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
    
    }
  }
  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }
}
