import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Producer } from '../Models/producer/producer.model';
import { CreateProducer } from '../Models/producer/create-producer.model';

@Injectable({
  providedIn: 'root'
})
export class ProducerService {
  baseUrl:string = environment.apiUrl + '/api/producer';
  constructor(private http:HttpClient) { }

  getProducers(){
    return this.http.get<Producer[]>(this.baseUrl + '/producers');
  }
  createProducer(model:CreateProducer){
    return this.http.post(this.baseUrl + '/producers',model)
  }
}
