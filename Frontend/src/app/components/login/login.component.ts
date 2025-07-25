import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  credentials = {
    username: '',
    password: ''
  };

  error: string = '';
  loginAttempts: number = 0;
  isLocked: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const role = localStorage.getItem('role');

    if (token && role) {
      try {
        const decodedToken: any = jwtDecode(token);
        const currentTime = Date.now() / 1000;

        if (decodedToken.exp && decodedToken.exp < currentTime) {
          // Token has expired
          localStorage.clear();
          return;
        }

        const dashboardRoute = role === 'Admin'
          ? '/adminDashboard'
          : role === 'User'
          ? '/userDashboard'
          : '/';

        this.router.navigate([dashboardRoute]);
      } catch (error) {
        console.error('Invalid token:', error);
        localStorage.clear(); // Remove invalid token
      }
    }
  }

  login() {
    if (this.isLocked) {
      this.error = 'Your account is locked due to multiple failed login attempts.';
      this.router.navigate(['/account-locked']);
      return;
    }

    this.authService.login(this.credentials).subscribe({
      next: (res) => {
        this.error = 'Login successful!';
        localStorage.setItem('token', res.token);
        const decodedToken: any = jwtDecode(res.token);
        localStorage.setItem('role', decodedToken.role);
        localStorage.setItem('accountNumber', decodedToken.accountNumber);
        localStorage.setItem('userId', res.authId);

        const dashboardRoute = decodedToken.role === 'Admin'
          ? '/adminDashboard'
          : decodedToken.role === 'User'
          ? '/userDashboard'
          : '/';

        this.router.navigate([dashboardRoute]);
        this.loginAttempts = 0;
      },
      error: (err) => {
        this.loginAttempts++;
        this.error = 'Login failed. Please check your credentials.';

        if (this.loginAttempts >= 3) {
          this.isLocked = true;
          this.router.navigate(['/account-locked']);
        }
      }
    });
  }
}
