import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  pendingAccounts: any[] = [];
  message = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadPendingAccounts();
  }

  loadPendingAccounts() {
    const url = 'https://localhost:7119/api/Admin/pending-requests';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    this.http.get<any[]>(url, { headers, withCredentials: true }) // ✅ Pass headers
      .subscribe({
        next: data => this.pendingAccounts = data,
        error: () => this.message = 'Failed to load pending accounts.'
      });
  }

  approve(requestId: number) {
    debugger
    const url = `https://localhost:7119/api/Admin/approve-account/${requestId}`;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    this.http.post(url, {}, { headers, withCredentials: true }) // ✅ Pass empty body and headers
      .subscribe({
        next: () => {
          this.message = 'Account approved!';
          this.loadPendingAccounts();
        },
        error: () => this.message = 'Approval failed.'
      });
  }

  reject(requestId: number) {
    const url = `http://localhost:7119/api/Admin/reject-account/${requestId}`;
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('token')}`);
    this.http.post(url, {}, { headers, withCredentials: true }) // ✅ Add missing headers
      .subscribe({
        next: () => {
          this.message = 'Account rejected!';
          this.loadPendingAccounts();
        },
        error: () => this.message = 'Rejection failed.'
      });
  }
}
