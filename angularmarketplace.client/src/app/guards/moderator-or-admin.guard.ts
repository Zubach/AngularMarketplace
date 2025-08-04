import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const moderatorOrAdminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  let res:boolean = authService.isLoggedIn() && (authService.getUserRole() == 'Moderator' || authService.getUserRole() == 'Admin');

  if(!res)
    router.navigateByUrl('/');
  return res;
};
