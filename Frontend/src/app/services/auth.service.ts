import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { AuthDTO, LoginDTO, LoginResponse, RegisterResponse, User } from '../models/user.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7119/api/Auth';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    // Check if user is already logged in on service initialization
    this.checkCurrentUser();
  }

  private checkCurrentUser(): void {
    const token = localStorage.getItem('token');
    const role = localStorage.getItem('role');
    const userId = localStorage.getItem('userId');
    const username = localStorage.getItem('username');
    const mobileNumber = localStorage.getItem('mobileNumber');

    if (token && this.isTokenValid(token) && role && userId && username) {
      console.log('Restoring user session from localStorage', { userId, username, role });
      const user: User = {
        authId: parseInt(userId),
        username: username,
        role: role as 'Admin' | 'User',
        mobileNumber: mobileNumber || ''
      };
      this.currentUserSubject.next(user);
    } else {
      console.log('No valid user session found, clearing localStorage');
      // If token is invalid or missing data, clear everything
      localStorage.clear();
      this.currentUserSubject.next(null);
    }
  }

  register(user: AuthDTO): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, user);
  }

  login(credentials: LoginDTO): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          if (response.success && response.data?.token) {
            this.setSession(response);
          }
        })
      );
  }

  private setSession(response: LoginResponse): void {
    const decodedToken: any = jwtDecode(response.data.token);
    
    localStorage.setItem('token', response.data.token);
    localStorage.setItem('role', decodedToken.role);
    localStorage.setItem('userId', response.data.authId.toString());
    localStorage.setItem('username', decodedToken.unique_name || decodedToken.sub);
    localStorage.setItem('accountNumber', decodedToken.accountNumber || '');
    localStorage.setItem('mobileNumber', decodedToken.mobileNumber || '');

    const user: User = {
      authId: response.data.authId,
      username: decodedToken.unique_name || decodedToken.sub,
      role: decodedToken.role,
      mobileNumber: decodedToken.mobileNumber || ''
    };

    this.currentUserSubject.next(user);
  }

  logout(): void {
    localStorage.clear();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return token ? this.isTokenValid(token) : false;
  }

  private isTokenValid(token: string): boolean {
    try {
      const decodedToken: any = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      return decodedToken.exp > currentTime;
    } catch (error) {
      return false;
    }
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  getUserRole(): string | null {
    return localStorage.getItem('role');
  }

  getUserId(): number | null {
    const userId = localStorage.getItem('userId');
    return userId ? parseInt(userId) : null;
  }

  getAccountNumber(): string | null {
    return localStorage.getItem('accountNumber');
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  isUser(): boolean {
    return this.getUserRole() === 'User';
  }

  private getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }
}