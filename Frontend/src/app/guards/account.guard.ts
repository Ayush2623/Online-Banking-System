import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountGuard implements CanActivate {
  constructor(private http: HttpClient, private router: Router) {}

  canActivate(): Observable<boolean> {
    const token = localStorage.getItem('token');
    const userId = localStorage.getItem('userId'); // Must be saved at login

    if (!token || !userId) {
      this.router.navigate(['/login']);
      return of(false);
    }

    const accountNumber = localStorage.getItem('accountNumber');
    if (accountNumber) {
      return of(true);
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<any>(`https://localhost:7119/api/Dashboard/viewAccount/${userId}`, { headers }).pipe(
      map(response => {
        if (response?.accountNumber) {
          localStorage.setItem('accountNumber', response.accountNumber);
          return true;
        } else {
          this.router.navigate(['/open-account']);
          return false;
        }
      }),
      catchError(() => {
        this.router.navigate(['/open-account']);
        return of(false);
      })
    );
  }
}
