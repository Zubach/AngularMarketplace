import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { FirtsKeyPipe } from '../../pipes/firts-key.pipe';
import { AuthService } from '../../services/auth.service';
import { UserRegistration } from '../../Models/user-registration.model';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule,FirtsKeyPipe],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {
  form: FormGroup;
  isSubmitted:Boolean = false;
  constructor(public formBuilder:FormBuilder,private authService:AuthService){
    this.form = this.formBuilder.group({
        fullName: ['',Validators.required],
        email: ['',[Validators.required,Validators.email]],
        password: ['',[Validators.required,Validators.minLength(6),this.passwordPattern]],//Validators.pattern('^(?=.*[A-Z])(?=.*[0-9])')]],  i dont know why its not working
        confirmPassword: ['']
      },{validators:this.passwordsMatchValidator});
   
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
        (response)=>{
          console.log(response);
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
