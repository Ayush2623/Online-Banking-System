import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { ApiResponse } from '../models/api.models';
import { Account } from '../models/account.models';

@Injectable({
  providedIn: 'root'
})
export class AccountGuard implements CanActivate {
  constructor(
    private http: HttpClient, 
    private router: Router,
    private authService: AuthService
  ) {}

  canActivate(): Observable<boolean> {
    // First, ensure user is properly authenticated
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return of(false);
    }

    const token = this.authService.getToken();
    const userId = this.authService.getUserId();

    if (!token || !userId) {
      this.router.navigate(['/login']);
      return of(false);
    }

    // Check if account number is already stored (from successful dashboard API call)
    const accountNumber = this.authService.getAccountNumber();
    if (accountNumber) {
      return of(true);
    }

    // Only hit Account API to check if user has an approved account
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<ApiResponse<Account>>(`https://localhost:7119/api/Account/view-account/${userId}`, { headers }).pipe(
      map(response => {
        if (response.success && response.data?.accountNumber) {
          localStorage.setItem('accountNumber', response.data.accountNumber);
          return true;
        } else {
          // User is authenticated but doesn't have an account - redirect to open account
          this.router.navigate(['/userDashboard/open-account']);
          return false;
        }
      }),
      catchError((error) => {
        console.error('Account validation failed:', error);
        // If account check fails, redirect to open account page
        this.router.navigate(['/userDashboard/open-account']);
        return of(false);
      })
    );
  }
}
