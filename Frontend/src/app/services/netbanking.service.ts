import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { 
  NetBankingDTO, 
  NetBankingRegistrationDTO,
  NetBankingUser,
  UpdateNetBankingPasswordRequest, 
  NetBankingDetails 
} from '../models/netbanking.models';

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
}

@Injectable({
  providedIn: 'root'
})
export class NetBankingService {
  private apiUrl = 'https://localhost:7119/api/NetBanking';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  registerForNetBanking(request: NetBankingDTO): Observable<string> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/register`, request, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => {
        if (response.success) {
          return response.message;
        }
        throw new Error(response.message);
      }),
      catchError(error => {
        const errorMessage = error?.error?.message || error.message || 'NetBanking registration failed';
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  registerNetBanking(request: NetBankingRegistrationDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, request, { 
      headers: this.getAuthHeaders() 
    });
  }

  getNetBankingUser(accountNumber: string): Observable<NetBankingUser> {
    return this.http.get<NetBankingUser>(`${this.apiUrl}?accountNumber=${accountNumber}`, { 
      headers: this.getAuthHeaders() 
    });
  }

  changeNetBankingPassword(request: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/change-password`, request, { 
      headers: this.getAuthHeaders() 
    });
  }

  deactivateNetBanking(accountNumber: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/deactivate`, { accountNumber }, { 
      headers: this.getAuthHeaders() 
    });
  }

  updatePassword(request: UpdateNetBankingPasswordRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/update-password`, request, { 
      headers: this.getAuthHeaders() 
    });
  }

  getNetBankingDetails(accountNumber: string): Observable<NetBankingDetails> {
    return this.http.get<ApiResponse<NetBankingDetails>>(`${this.apiUrl}?accountNumber=${accountNumber}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => {
        if (response.success && response.data) {
          return response.data;
        }
        throw new Error(response.message || 'Failed to fetch NetBanking details');
      }),
      catchError(error => {
        const errorMessage = error?.error?.message || error.message || 'Failed to fetch NetBanking details';
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}