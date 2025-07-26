import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    // Use AuthService to properly validate authentication
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }

    const userRole = this.authService.getUserRole();
    const expectedRole = route.data['role'];
    
    if (expectedRole && expectedRole !== userRole) {
      console.warn(`Access denied. Expected role: ${expectedRole}, User role: ${userRole}`);
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
