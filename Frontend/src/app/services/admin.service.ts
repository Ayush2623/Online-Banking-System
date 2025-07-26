import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { PendingAccount } from '../models/account.models';
import { ApiResponse } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'https://localhost:7119/api/Admin';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getPendingRequests(): Observable<PendingAccount[]> {
    return this.http.get<ApiResponse<PendingAccount[]>>(`${this.apiUrl}/pending-requests`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data || [])
    );
  }

  approveAccount(requestId: number): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/approve-account/${requestId}`, {}, { 
      headers: this.getAuthHeaders() 
    });
  }

  rejectAccount(requestId: number): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/reject-account/${requestId}`, {}, { 
      headers: this.getAuthHeaders() 
    });
  }
}