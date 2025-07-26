import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { PayeeDTO, FundTransferRequest, Payee } from '../models/fund-transfer.models';
import { ApiResponse } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class FundTransferService {
  private apiUrl = 'https://localhost:7119/api/FundTransfer';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  addPayee(payee: PayeeDTO): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/add-payee`, payee, { 
      headers: this.getAuthHeaders() 
    });
  }

  transferFunds(request: FundTransferRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}/transfer-funds`, request, { 
      headers: this.getAuthHeaders() 
    });
  }

  getPayees(accountNumber: string): Observable<Payee[]> {
    return this.http.get<ApiResponse<Payee[]>>(`${this.apiUrl}/payees/${accountNumber}`, { 
      headers: this.getAuthHeaders() 
    }).pipe(
      map(response => response.data || [])
    );
  }
}