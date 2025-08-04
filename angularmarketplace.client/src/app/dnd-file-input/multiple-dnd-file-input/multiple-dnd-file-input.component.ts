import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DragNDropFileInput } from '../dnd-file-input.component';
import { ConvertService } from '../../services/convert.service';

@Component({
  selector: 'app-multiple-dnd-file-input',
  templateUrl: './multiple-dnd-file-input.component.html',
  styleUrl: './multiple-dnd-file-input.component.css',
  standalone:true,
  imports:[DragNDropFileInput]
})
export class MultipleDragNDropFileInputComponent {
  @Input() previewEnable:boolean = false;
  @Input() labelText!:string;
  @Input() annotation?:string;
  @Input() maxFilesCount:number = 6;


  imgsBase64?:string[];

  @Output() filesChanged:EventEmitter<FileList> = new EventEmitter();

  numberArr:number[] = (new Array(this.maxFilesCount - 1).fill(4)).map((x,y)=>y);

  constructor(private convert:ConvertService){}

  onFilesChanged(files:FileList){
    if(files && files?.length > 0){
      this.imgsBase64 = new Array();
      for(let i = 0; i < files.length; i++){
        this.convert.fileToBase64(files[i]).then((value:string)=>{
          this.imgsBase64?.push(value); 
        })
        .catch(error => console.log(error));
      }
    }
    this.filesChanged?.emit(files);
  }
}
