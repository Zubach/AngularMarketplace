import { FormGroup } from "@angular/forms";


declare module "@angular/forms"{
    interface FormGroup {
        hasDisplayableError(controlName:string,isFormSubmitted?:boolean): boolean;
    }
}

FormGroup.prototype.hasDisplayableError = (function(this: FormGroup,controlName:string,isFormSubmitted?:boolean):boolean{
    const control = this.get(controlName);
    return Boolean(control?.invalid) &&
      (!isFormSubmitted ? false : isFormSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
});
