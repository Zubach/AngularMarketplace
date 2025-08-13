import { Component, Input } from '@angular/core';
import { Producer } from '../../../../../Models/producer/producer.model';

@Component({
  selector: 'app-producer-table',
  templateUrl: './producer-table.component.html',
  styleUrl: './producer-table.component.css',
  standalone:true
})
export class ProducerTableComponent {
  @Input() producers!:Producer[];
  
}
