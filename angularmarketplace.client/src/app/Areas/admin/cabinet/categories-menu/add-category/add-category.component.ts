import { Component, ElementRef, inject, TemplateRef, ViewChild } from '@angular/core';
import { CategoriesService } from '../../../../../services/categories.service';
import { Category } from '../../../../../Models/category/category.model';
import { AuthService } from '../../../../../services/auth.service';
import { CreateCategory } from '../../../../../Models/category/create-category.model';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SelectCategoryComponent } from '../../../../../categories-list/select-category/select-category.component';
import {  NgbModal, NgbTypeaheadModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Svg } from '../../../../../Models/assets/svg.model';
import { AssetsService } from '../../../../../services/assets.service';
import { debounceTime, map, Observable, OperatorFunction } from 'rxjs';
import { ToastService } from '../../../../../services/toast.service';
import { NgClass } from '@angular/common';
import { DragNDropFileInput } from '../../../../../dnd-file-input/dnd-file-input.component';
@Component({
  selector: 'app-add-category',
  standalone:true,
  imports: [NgbTypeaheadModule, FormsModule, ReactiveFormsModule, SelectCategoryComponent, NgbModule,NgClass,DragNDropFileInput],
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css'
})
export class AddCategoryComponent {
  private modalService = inject(NgbModal);

  @ViewChild('content') modalFormComponent!:ElementRef;
  @ViewChild('selectCategory') selectCategoryComponent!: SelectCategoryComponent;
  @ViewChild('createdSuccessToast') createdSuccessToast!: TemplateRef<any>;
  @ViewChild('selectCatModal') selectCategoryModal!: ElementRef;

  model?:CreateCategory;
  categoriesList!:Category[];
  form:FormGroup;
  isSubmitted:boolean = false;
  logoBase64?:string;
  svg?: Svg;
  svgs_list:Svg[] = [];

  categoryBtnHTML:string = "Select category";
  categoryBtnClass: string = "btn btn-primary"

  constructor(
    private assetsService:AssetsService,
    private formBuilder:FormBuilder,
    private authService:AuthService,
    private categoriesService:CategoriesService,
    private toastService:ToastService
  ) { 
    this.form = this.formBuilder.group({
          title:['',Validators.required],
          url_title:['',Validators.required],
          parentCategory: new FormControl<null | Category>(null),
          logo: new FormControl<null | File>(null)
        });
    this.assetsService.getSvgsList().subscribe((data:Svg[])=>{this.svgs_list = data;});
  }

  showModalForm(){
    this.form.reset();
    this.modalService.open(this.modalFormComponent,{centered:true,scrollable:true});
    if(this.authService.isAdmin()){
      this.categoriesService.generateCreateCategoryModel().subscribe({
        next: data=>{
          this.model = data;
        },
        error: response=>{

        }
      });

      if(this.categoriesList == null){
         this.categoriesService.getMainCategories().subscribe((data:Category[])=>{
          this.categoriesList = data
        });
      }
    }
  }

  logoSelected(event:Event){
    const input = event.target as HTMLInputElement;
    if(input.files && input.files.length > 0){
      const file = input.files[0] as File;
      this.form.patchValue({logo:file});
      this.toBase64(file).then((value:string)=> {this.logoBase64 = value})
      .catch(error => console.log(error));
    }
  }

  logoChanged(files:FileList){
    console.log(files);
  }

  toBase64(file:File): Promise<string>{
    return new Promise((resolve,reject)=>{
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = ()=> resolve(reader.result as string);
      reader.onerror = (error)=> reject(error);
    });
  }

    onSubmit(){
      this.isSubmitted = true;
      if(this.form.valid){

        if(this.model){
          let parent = this.form.controls['parentCategory'].value as Category
          this.model.img = this.form.controls['logo'].value;
          this.model.isSubCategory = parent ? true : false;
          this.model.title = this.form.controls['title'].value;
          this.model.url_Title = this.form.controls['url_title'].value;
          this.model.parent = parent;
          if(this.svg && !this.model?.isSubCategory){
            this.model.svg = this.svg.file_name;
          }
          this.categoriesService.createCategory(this.model).subscribe({
            next: data =>{
              this.toastService.show({
                body: "Category succesfully created.",
                classname: "bg-success text-light"
              });
              this.modalService.dismissAll();
            },
            error: request =>{

            }
          });
        }
        
      }
   }

  categoryBtnClick(){
    if(this.form.controls['parentCategory'].value ){
      this.categoryBtnHTML = "Select Category";
      this.categoryBtnClass = "btn btn-primary";
      this.form.patchValue({parentCategory: undefined});
   
    }
    else{
     
      this.modalService.open(this.selectCategoryModal,{centered:true,scrollable:true});
    }
    
  }
  onChangeCategory(category:Category){
      this.form.patchValue({parentCategory:category});
      this.categoryBtnHTML = "Clear"
      this.categoryBtnClass = "btn btn-danger";
  }
  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }


  // search svgs
  search: OperatorFunction<string,readonly Svg[]> = (text$:Observable<string>)=>
      text$.pipe(
        debounceTime(200),
        map((term)=>
          term === ''
            ? []
            : this.svgs_list?.filter((s)=> s.name.toLowerCase().indexOf(term.toLowerCase())>-1).slice(0,10)

        )
      
    );
  formatter = (x:{name:string})=> x.name
}
