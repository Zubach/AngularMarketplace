import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConvertService {

  constructor() { }

  fileToBase64(file:File): Promise<string>{
    return new Promise((resolve,reject)=>{
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = ()=> resolve(reader.result as string);
      reader.onerror = (error)=> reject(error);
    });
  }
  objectToFormData(object:object):FormData{
    const formData = new FormData();
    Object.entries(object).forEach(([key,value])=>{
         if(typeof value ==='object' && !(value instanceof File) && !(value instanceof FileList)){
            formData.append(key,JSON.stringify(value));

         }
         else if(value instanceof FileList){
            Array.from(value).map((file,i)=>{
              return formData.append('file' + i,file,file.name);
            })
         }
         else
           formData.append(key,value);
       });
    return formData;
  }
}
