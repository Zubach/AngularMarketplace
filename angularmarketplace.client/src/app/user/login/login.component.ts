import { Component, ElementRef, OnDestroy, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FirtsKeyPipe } from '../../pipes/firts-key.pipe';
import { UserLogin } from '../../Models/User/user-login';
import { AuthService } from '../../services/auth.service';
import { ToastContainer } from '../../toast-container/toast-container.component';
import { ToastService } from '../../services/toast.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports:[ReactiveFormsModule,ToastContainer]
})
export class LoginComponent implements OnDestroy {
  form:FormGroup;
  isSubmitted:Boolean = false;
  @ViewChild("loginFailed") loginFailedEl!:TemplateRef<any>;

  constructor(public formBuilder: FormBuilder,private authService:AuthService,private toastService:ToastService, private router:Router){
    this.form = this.formBuilder.group({
              email: ['',[Validators.required,Validators.email]],
              password: ['',[Validators.required]]
    });
  }
  ngOnDestroy(): void {
    this.toastService.clear();
  }

  onSubmit(){
    if(this.form.valid){
      this.isSubmitted = true;
      let usr:UserLogin={
        email: this.form.controls['email'].value,
        password: this.form.controls['password'].value
      };
      this.authService.login(usr).subscribe({
        next:(response:any)=>{

          this.authService.saveToken(response.token);
          switch(this.authService.getUserRole()){
            case "Admin":
              this.router.navigateByUrl("/admin-cabinet");
              break;
            case "Seller":
              this.router.navigateByUrl("/sellermenu");
              break;
            default:
              this.router.navigateByUrl("/");
              break;
          }
        },
        error: err=>{
          if(err.status == 400){
            
            this.toastService.show(
              {
                body:this.loginFailedEl,
                classname: 'bg-danger text-light',
                delay: 5000
              }
            )
          }
        }
      });
    }
  }
  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }
}
