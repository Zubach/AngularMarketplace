import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { FirtsKeyPipe } from '../../pipes/firts-key.pipe';

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
  constructor(public formBuilder:FormBuilder){
    this.form = this.formBuilder.group({
        fullName: ['',Validators.required],
        email: ['',[Validators.required,Validators.email]],
        password: ['',[Validators.required,Validators.minLength(6)]],
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
  
  onSubmit(){
    this.isSubmitted = true;
  }

  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }
}
