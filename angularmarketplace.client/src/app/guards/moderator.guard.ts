import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const moderatorGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  let res:boolean = authService.isLoggedIn() && authService.getUserRole() == 'Moderator';
  
  if(!res){
    router.navigateByUrl('/');
  }

  return res;
};
