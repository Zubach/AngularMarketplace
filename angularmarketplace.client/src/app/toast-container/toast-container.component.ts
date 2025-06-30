
import { Component, inject } from '@angular/core';
import { ToastService } from '../services/toast.service';
import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgTemplateOutlet } from '@angular/common';
@Component({
  selector: 'app-toasts',
  standalone:true,
  imports:[NgbToastModule, NgTemplateOutlet],
  templateUrl: './toast-container.component.html',
  host: { class: 'toast-container position-fixed top-0 end-0 p-3', style: 'z-index: 1200' },
})
export class ToastContainer {
  readonly toastService = inject(ToastService);

}