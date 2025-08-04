import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Svg } from '../Models/assets/svg.model';

@Injectable({
  providedIn: 'root'
})
export class AssetsService {

  constructor(private http:HttpClient) {   }

  getSvgsList(){
    return this.http.get<Svg[]>('/assets/svgs/svgs_list.json');
  }
}
