import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { AccountService } from '../../../../services/account.service';
import { NetBankingService } from '../../../../services/netbanking.service';
import { Account } from '../../../../models/account.models';
import { NetBankingDTO, NetBankingDetails } from '../../../../models/netbanking.models';

@Component({
  selector: 'app-net-banking',
  templateUrl: './net-banking.component.html',
  styleUrls: ['./net-banking.component.scss']
})
export class NetBankingComponent implements OnInit {
  registrationForm!: FormGroup;
  changePasswordForm!: FormGroup;
  isLoading = false;
  error: string = '';
  successMessage: string = '';
  currentUser: any;
  userAccount: Account | null = null;
  netBankingDetails: NetBankingDetails | null = null;
  isRegistered = false;
  showRegistrationForm = false;
  showPassword = false;
  showConfirmPassword = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private accountService: AccountService,
    private netBankingService: NetBankingService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.initializeForms();
    this.loadUserAccount();
  }

  private initializeForms(): void {
    this.registrationForm = this.formBuilder.group({
      accountNumber: ['', [Validators.required]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { 
      validators: [this.passwordMatchValidator]
    });
  }

  private passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else if (confirmPassword?.hasError('passwordMismatch')) {
      confirmPassword.setErrors(null);
    }
    
    return null;
  }


  private loadUserAccount(): void {
    const userId = this.authService.getUserId();
    if (!userId) {
      this.router.navigate(['/login']);
      return;
    }

    this.accountService.viewAccountByUserId(userId).subscribe({
      next: (account) => {
        this.userAccount = account;
        this.registrationForm.patchValue({ accountNumber: account.accountNumber });
        this.checkNetBankingStatus();
      },
      error: (err) => {
        this.error = 'Failed to load account information.';
        console.error('Error loading account:', err);
      }
    });
  }

  private checkNetBankingStatus(): void {
    if (!this.userAccount) return;

    this.netBankingService.getNetBankingDetails(this.userAccount.accountNumber).subscribe({
      next: (netBankingDetails: NetBankingDetails) => {
        this.netBankingDetails = netBankingDetails;
        this.isRegistered = true;
      },
      error: (err: any) => {
        if (err.status === 404) {
          this.isRegistered = false;
        } else {
          console.error('Error checking net banking status:', err);
        }
      }
    });
  }

  toggleRegistrationForm(): void {
    this.showRegistrationForm = !this.showRegistrationForm;
    if (!this.showRegistrationForm) {
      this.registrationForm.reset();
      this.registrationForm.patchValue({ accountNumber: this.userAccount?.accountNumber });
      this.error = '';
      this.successMessage = '';
    }
  }

  onRegistrationSubmit(): void {
    if (this.registrationForm.valid && this.userAccount) {
      this.isLoading = true;
      this.error = '';
      this.successMessage = '';

      const registrationData: NetBankingDTO = {
        AccountNumber: parseInt(this.userAccount.accountNumber),
        Username: this.registrationForm.value.username,
        Password: this.registrationForm.value.password
      };

      this.netBankingService.registerForNetBanking(registrationData).subscribe({
        next: (message: string) => {
          this.isLoading = false;
          this.successMessage = message;
          this.showRegistrationForm = false;
          this.isRegistered = true;
          this.checkNetBankingStatus();
        },
        error: (err: any) => {
          this.isLoading = false;
          this.error = err.message || 'Failed to register for Net Banking. Please try again.';
        }
      });
    } else {
      this.markFormGroupTouched(this.registrationForm);
    }
  }


  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      formGroup.get(key)?.markAsTouched();
    });
  }

  getFieldError(formGroup: FormGroup, fieldName: string): string {
    const field = formGroup.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required`;
      }
      if (field.errors['minlength']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['passwordMismatch']) {
        return 'Passwords do not match';
      }
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      accountNumber: 'Account Number',
      username: 'Username',
      password: 'Password',
      confirmPassword: 'Confirm Password'
    };
    return displayNames[fieldName] || fieldName;
  }

  togglePasswordVisibility(field: string): void {
    if (field === 'password') {
      this.showPassword = !this.showPassword;
    } else if (field === 'confirmPassword') {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  formatDate(dateString: string): string {
    try {
      return new Date(dateString).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    } catch (error) {
      return dateString;
    }
  }

  goBack(): void {
    this.router.navigate(['/userDashboard']);
  }
}