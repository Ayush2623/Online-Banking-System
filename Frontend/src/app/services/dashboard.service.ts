import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { 
  DashboardData, 
  AccountSummary, 
  AccountStatement, 
  ChangePasswordRequest,
  Transaction 
} from '../models/dashboard.models';
import { ApiResponse } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'https://localhost:7119/api/Dashboard';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getDashboard(accountNumber: string): Observable<DashboardData> {
    return this.http.get<ApiResponse<DashboardData>>(`${this.apiUrl}/dashboard?accountNumber=${accountNumber}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data!)
    );
  }

  getAccountSummary(accountNumber: string): Observable<AccountSummary> {
    return this.http.get<ApiResponse<AccountSummary>>(`${this.apiUrl}/account-summary/account-number?accountNumber=${accountNumber}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data!)
    );
  }

  getAccountStatement(accountNumber: string, startDate: string, endDate: string): Observable<Transaction[]> {
    // Format dates for the API (ensure proper ISO format)
    const formattedStartDate = new Date(startDate).toISOString();
    const formattedEndDate = new Date(endDate).toISOString();
    
    const params = `AccountNumber=${accountNumber}&startDate=${formattedStartDate}&endDate=${formattedEndDate}`;
    return this.http.get<ApiResponse<Transaction[]>>(`${this.apiUrl}/account-statement/Account Number?${params}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data || [])
    );
  }

  getAllTransactions(accountNumber: string): Observable<Transaction[]> {
    return this.http.get<ApiResponse<Transaction[]>>(`${this.apiUrl}/transactions/${accountNumber}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data || [])
    );
  }

  changePassword(request: ChangePasswordRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/change-password`, request, { 
      headers: this.getAuthHeaders() 
    });
  }

  getSessionExpired(): Observable<any> {
    return this.http.get(`${this.apiUrl}/session-expired`, { 
      headers: this.getAuthHeaders() 
    });
  }
}