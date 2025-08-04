
import { Component, inject, TemplateRef } from '@angular/core';
import { Toast, ToastService } from '../services/toast.service';
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

  isBodyString(toast:Toast):boolean{
    return toast.body instanceof TemplateRef;
  }
  bodyToTemplate(toast:Toast):TemplateRef<any>{
    return toast.body as TemplateRef<any>;
  }
}