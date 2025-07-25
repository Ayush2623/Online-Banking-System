import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent implements OnInit {
  accountDetails: any = null;
  message = '';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    const userId = localStorage.getItem('userId');
    const token = localStorage.getItem('token');

    if (!userId || !token) {
      this.router.navigate(['/login']);
      return;
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>(`https://localhost:7119/api/Account/view-account/${userId}`, { headers }).subscribe({
      next: (res) => {
        if (res.accountNumber) {
          localStorage.setItem('accountNumber', res.accountNumber);
          this.fetchUserProfile(res.accountNumber, headers);
        } else {
          this.router.navigate(['/user-dashboard/open-account']);
        }
      },
      error: () => {
        this.router.navigate(['/user-dashboard/open-account']);
      }
    });
  }

  fetchUserProfile(accountNumber: string, headers: HttpHeaders): void {
  const params = new HttpParams().set('accountNumber', accountNumber);

  this.http.get<any>(
    'https://localhost:7119/api/Dashboard/dashboard',
    { headers, params }
  ).subscribe({
    next: data => this.accountDetails = data,
    error: err => this.message = 'Failed to load account details.'
  });
}
}
