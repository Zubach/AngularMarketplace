import { Component, ElementRef, EventEmitter, Input, Output, Renderer2, ViewChild } from '@angular/core';
import {state, style, trigger} from '@angular/animations'
import { ConvertService } from '../services/convert.service';

@Component({
  selector: 'app-dnd-file-input',
  templateUrl: './dnd-file-input.component.html',
  styleUrl: './dnd-file-input.component.css',
  standalone:true
})
export class DragNDropFileInput {
  @Input() labelText!:string;
  @Input() accept!:string;
  @Input() annotation?:string;
  @Input() previewEnable:boolean = false;

  imgBase64?:string;
  fileName?:string;
  
  @Output() filesChanged:EventEmitter<FileList> = new EventEmitter();

  isDragOver:boolean = false;
  isFilesDropped:boolean = false;

  @ViewChild('container') container!:ElementRef<any>;
  @ViewChild('fileInput') fileInput!:HTMLInputElement;
  constructor(private renderer:Renderer2,private convert:ConvertService){}

  onDragOver(event:Event){
    event.preventDefault();
    this.renderer.addClass(this.container.nativeElement,"drag-over");
    this.isDragOver = true;
  }
  onDragLeave(event:Event){
    event.preventDefault();
    this.renderer.removeClass(this.container.nativeElement,"drag-over");
    this.isDragOver = false;
  }
  changeFiles(files:FileList){
    if(this.previewEnable && files && files.length > 0){
      this.convert.fileToBase64(files[0]).then((value:string)=> {
        this.imgBase64 = value;
        this.fileName = files[0].name;
        this.renderer.addClass(this.container.nativeElement,"img-preview");
      })
      .catch(error => console.log(error));

    }
    this.filesChanged?.emit(files);
  }
  onDrop(event:DragEvent){
    event.preventDefault();
    if(event.dataTransfer?.files.length){
      const files = event.dataTransfer.files;
      this.fileInput.files = files;

      this.changeFiles(files);
    }
    
  }
  onFileChange(event:Event){
    const inp = event.target as HTMLInputElement;
    if(inp.files && inp.files?.length > 0){
      this.changeFiles(inp.files);
    }
  }



}
