import { Component, OnDestroy, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { FirtsKeyPipe } from '../../pipes/firts-key.pipe';
import { AuthService } from '../../services/auth.service';
import { UserRegistration } from '../../Models/User/user-registration.model';
import { ToastContainer } from "../../toast-container/toast-container.component";
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule, FirtsKeyPipe, ToastContainer],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent implements OnDestroy {
  form: FormGroup;
  isSubmitted:Boolean = false;
  @ViewChild("registrationFailed") regFailedEl!: TemplateRef<any>;
  errorMsg:string = '';
  constructor(public formBuilder:FormBuilder,private authService:AuthService,private toastService:ToastService){
    this.form = this.formBuilder.group({
        fullName: ['',Validators.required],
        email: ['',[Validators.required,Validators.email]],
        password: ['',[Validators.required,Validators.minLength(6),this.passwordPattern]],
        confirmPassword: ['']
      },{validators:this.passwordsMatchValidator});
   
  }
  ngOnDestroy(): void {
    this.toastService.clear();
  }
  passwordsMatchValidator: ValidatorFn = (control: AbstractControl): null =>{
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if(password && confirmPassword && password.value != confirmPassword.value)
      confirmPassword?.setErrors({passwordMismatch: true});
    else
      confirmPassword?.setErrors(null);
    return null;
  };

  passwordPattern:ValidatorFn = (control: AbstractControl): ValidationErrors | null =>{
    const value = control.value;
    if(!value){
      return null;
    }
    const hasUppercase = /[A-Z]/.test(value);
    const hasDigit = /[0-9]/.test(value);

    const isValid = hasDigit && hasUppercase;
    return isValid ? null : {patternError: true};
  }
  
  onSubmit(){
    if(this.form.valid){
      this.isSubmitted = true;
      let usr:UserRegistration = {
        fullName : this.form.controls['fullName'].value,
        email : this.form.controls['email'].value,
        password : this.form.controls['password'].value
      };

      this.authService.register(usr).subscribe(
        {
          next:response=>{
            console.log(response);
          },
          error: err=>{
            this.errorMsg = '';
            console.log(err);
            if(err.custom_message == null){
              err.error.errors.forEach((e:any) => {
                this.errorMsg += e.description + '</br>'
              });
            }
            else{
              this.errorMsg = err.custom_message;
            }
            this.toastService.show({
              template: this.regFailedEl,
              classname: 'bg-danger text-light',
              delay: 5000
            });
          }
        }
      );
    }
  }

  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }
}
