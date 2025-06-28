import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FirtsKeyPipe } from '../../pipes/firts-key.pipe';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports:[ReactiveFormsModule]
})
export class LoginComponent {
  form:FormGroup;
  isSubmitted:Boolean = false;
  constructor(public formBuilder: FormBuilder){
    this.form = this.formBuilder.group({
              email: ['',[Validators.required,Validators.email]],
              password: ['',[Validators.required]]
    });
  }

  onSubmit(){
    if(this.form.valid){
      this.isSubmitted = true;

      console.log(this.form.value);
    }
  }
  hasDisplayableError(controlName:string):Boolean{
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }
}
