import { Component, computed, ElementRef, EventEmitter, Input, Output, signal, Signal, ViewChild, WritableSignal } from '@angular/core';
import { Category } from '../../../../../Models/category/category.model';
import { Checkbox } from '../../../../../Models/checkbox/checkbox.model';
import { ConvertService } from '../../../../../services/convert.service';


@Component({
  selector: 'app-admin-select-categories',
  templateUrl: './select-categories.component.html',
  styleUrl: './select-categories.component.css',
  standalone:true,
  
})
export class SelectCategoriesComponent {
  @Input() checkBoxes!:WritableSignal<Checkbox>;
  @Input() checked!:boolean;

  @Output() onChange:EventEmitter<number[]> = new EventEmitter();
  
  constructor(public converter:ConvertService){
  }

  changeCategories(){ 
    let result: number[] = [];
    this.getChecked(this.checkBoxes(),result);
    this.onChange?.emit(result);
  }

  getChecked(checkBox:Checkbox,checked_arr:number[]):number[]{
    if(checkBox.checked)
      if(checkBox.id)
      checked_arr.push(checkBox.id);
    if(checkBox.children)
      checkBox.children.forEach(element => {
        return this.getChecked(element,checked_arr);
      });
    return checked_arr;
  }

  update(event:Event,index?:number){
    
    const isChecked = ((event as Event).target as HTMLInputElement).checked;
    
    if(this.checkBoxes){
      this.checkBoxes.update(signal =>{
        if(index === undefined){
          signal.checked = isChecked;
          this.updateChildren(signal,isChecked);
          
        }
        else{
          signal.children![index].checked = isChecked;
          signal.checked = signal.children?.every(t => t.checked) ?? true;
        }
        return {...signal};
      });
    }
    this.changeCategories();
  }
  partiallyChecked = computed(()=>{
      const signal = (this.checkBoxes as WritableSignal<Checkbox>)();
      if(!signal.children)
        return false;
      return signal.children.some(c => c.checked) && !signal.children.every(c=> c.checked);
  });
  convertToSignal(checkBox:Checkbox):WritableSignal<Checkbox> {
      return signal(checkBox);
  }
  updateChildren(checkBox:Checkbox,checked:boolean){
    if(checkBox.children){
      checkBox.children.forEach(c =>{
        c.checked = checked;
        if(c.children)
          this.updateChildren(c,checked);
      });
    }
  }
}
