import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { LoginDTO } from '../../models/user.models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  error: string = '';
  successMessage: string = '';
  isLoading: boolean = false;
  loginAttempts: number = 0;
  isLocked: boolean = false;
  showPassword: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService, 
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.checkIfAlreadyLoggedIn();
  }

  private initializeForm(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  private checkIfAlreadyLoggedIn(): void {
    if (this.authService.isLoggedIn()) {
      const role = this.authService.getUserRole();
      const dashboardRoute = role === 'Admin' ? '/adminDashboard' : '/userDashboard';
      this.router.navigate([dashboardRoute]);
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.loginForm.valid && !this.isLocked) {
      this.isLoading = true;
      this.error = '';
      this.successMessage = '';

      const credentials: LoginDTO = this.loginForm.value;

      this.authService.login(credentials).subscribe({
        next: (response) => {
          this.isLoading = false;
          if (response.success) {
            this.successMessage = response.message || 'Login successful! Redirecting...';
            this.loginAttempts = 0;
            this.error = '';
            
            setTimeout(() => {
              const role = this.authService.getUserRole();
              const dashboardRoute = role === 'Admin' ? '/adminDashboard' : '/userDashboard';
              this.router.navigate([dashboardRoute]);
            }, 1000);
          } else {
            this.error = response.message || 'Login failed. Please check your credentials.';
            this.loginAttempts++;
            if (this.loginAttempts >= 3) {
              this.isLocked = true;
              this.error = 'Account locked due to multiple failed attempts. Please try again later.';
              this.resetLockout();
            }
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.loginAttempts++;
          this.error = err.error?.message || 'Login failed. Please check your credentials.';

          if (this.loginAttempts >= 3) {
            this.isLocked = true;
            this.error = 'Account locked due to multiple failed attempts. Please try again later.';
            this.resetLockout();
          }
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.loginForm.controls).forEach(key => {
      this.loginForm.get(key)?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.loginForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} is required`;
      }
      if (field.errors['minlength']) {
        return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
    }
    return '';
  }

  resetLockout(): void {
    setTimeout(() => {
      this.isLocked = false;
      this.loginAttempts = 0;
      this.error = '';
    }, 30000); // 30 seconds lockout
  }
}