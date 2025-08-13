import { Component, ElementRef, EventEmitter, Input,Output, signal, ViewChild, WritableSignal } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category } from '../../../../../Models/category/category.model';
import "../../../../../extensions/form-group.extensions"
import { SelectCategoriesComponent } from '../select-categories/select-categories.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Checkbox } from '../../../../../Models/checkbox/checkbox.model';
import { ConvertService } from '../../../../../services/convert.service';
import { CreateProducer } from '../../../../../Models/producer/create-producer.model';
import { ProducerService } from '../../../../../services/producer.service';
import { ToastService } from '../../../../../services/toast.service';

@Component({
  selector: 'app-add-producer',
  templateUrl: './add-producer.component.html',
  styleUrl: './add-producer.component.css',
  standalone:true,
  imports: [ReactiveFormsModule,SelectCategoriesComponent]
})
export class AddProducerComponent {

  form:FormGroup;
  isSubmitted: boolean = false;
  @Input() categories!:Category[];
  @ViewChild('content') modalFormComponent!:ElementRef;
  checkBoxSignal!: WritableSignal<Checkbox>;
  @Output() onCreated:EventEmitter<Object> = new EventEmitter();


  constructor(
    private formBuilder:FormBuilder,
    private modalService:NgbModal,
    private converter:ConvertService,
    private producerService:ProducerService,
    private toastService:ToastService
  ){
    this.form = this.formBuilder.group({
      name: ['',Validators.required],
      categories: new FormControl<null|Category[]>(null)
    });
    
  }

  onSubmit(){
    this.isSubmitted = true;
    if(this.form.valid){
      let model:CreateProducer={
        name:this.form.controls['name'].value,
        categories: this.form.controls['categories'].value as number[]
      }
      this.producerService.createProducer(model).subscribe({
        next: response=>{
          this.toastService.show({
            body: "Created successfully",
            classname: "bg-success text-light",
            delay:500
          });
          this.modalService.dismissAll();
          this.onCreated?.emit(response);
        },
        error: err =>{
          this.toastService.show({
            body: err.message,
            classname: "bg-danger text-light",
            delay: 500
          });
        }
      });
    }
  }

  showModalForm(){
    this.form.reset();
    this.isSubmitted = false;
    this.modalService.open(this.modalFormComponent,{centered:true,scrollable:true});
    if(!this.checkBoxSignal){
      let root:Checkbox = {
        title: 'Root',
        checked: false,
        children: []
      };
      root.children = this.categories.map(c => this.converter.categoryToCategoryCheckbox(c));
      this.checkBoxSignal = signal(root);
    }
  }

  categoriesOnChange(categories:number[]){
    this.form.patchValue({categories:categories});
  }
}
