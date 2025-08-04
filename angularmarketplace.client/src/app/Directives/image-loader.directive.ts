import { booleanAttribute, Directive, ElementRef, inject, Input, OnInit } from '@angular/core';
import { environment } from '../../environments/environment';

@Directive({
  selector: 'img[appImageLoader]'
})
export class ImageLoaderDirective implements OnInit {
    private el:ElementRef<HTMLImageElement> =  inject(ElementRef);
  
  
    @Input({transform: booleanAttribute}) priority = false;
    @Input()load_from?: string = 'products'; 

  constructor() { }

    ngOnInit(): void {
      /* Add cdnUrl to imgName for make fullPath */
      let url = environment.cdnUrl;
              let queryParams = [];  
              switch(this.load_from){
                case 'products':
                case 'logos':
                case 'categories':
                  queryParams.push('/' + this.load_from + '/');
                  break;
                default:
                    break;
              }
      this.el.nativeElement.setAttribute('src',url + queryParams.join('/') + this.el.nativeElement.getAttribute('src'));
        if(!this.priority){
          this.el.nativeElement.setAttribute('loading','lazy');
        }

    }

}
