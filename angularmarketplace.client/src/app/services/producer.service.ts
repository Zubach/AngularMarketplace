import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Producer } from '../Models/producer/producer.model';
import { CreateProducer } from '../Models/producer/create-producer.model';

@Injectable({
  providedIn: 'root'
})
export class ProducerService {
  baseUrl:string = environment.apiUrl + '/api/producers/';
  constructor(private http:HttpClient) { }

  getProducers(){
    return this.http.get<Producer[]>(this.baseUrl);
  }
  createProducer(model:CreateProducer){
    return this.http.post(this.baseUrl ,model)
  }
  getCategoryProducersById(categoryId:number){
    return this.http.get<Producer[]>(this.baseUrl + 'category/' + categoryId);
  }
}
