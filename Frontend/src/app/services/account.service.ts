import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { 
  Account, 
  PendingAccountDTO, 
  PendingAccount, 
  UpdateAccountDTO, 
  ForgotPasswordRequest, 
  ForgotUserIdRequest, 
  SetNewPasswordRequest 
} from '../models/account.models';
import { ApiResponse } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private apiUrl = 'https://localhost:7119/api/Account';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  openAccount(accountData: PendingAccountDTO): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/open-account`, accountData, { 
      headers: this.getAuthHeaders() 
    });
  }

  viewAccountByUserId(userId: number): Observable<Account> {
    return this.http.get<ApiResponse<Account>>(`${this.apiUrl}/view-account/${userId}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data!)
    );
  }

  updateAccount(accountNumber: string, updatedAccount: UpdateAccountDTO): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}/update-account/${accountNumber}`, updatedAccount, { 
      headers: this.getAuthHeaders() 
    });
  }

  forgotPassword(request: ForgotPasswordRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/forgot-password`, request);
  }

  forgotUserId(request: ForgotUserIdRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/forgot-user-id`, request);
  }

  setNewPassword(request: SetNewPasswordRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/set-new-password`, request);
  }
}