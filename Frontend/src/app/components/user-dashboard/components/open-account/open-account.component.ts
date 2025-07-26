import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { AccountService } from '../../../../services/account.service';
import { PendingAccountDTO } from '../../../../models/account.models';

@Component({
  selector: 'app-open-account',
  templateUrl: './open-account.component.html',
  styleUrls: ['./open-account.component.scss']
})
export class OpenAccountComponent implements OnInit {
  openAccountForm!: FormGroup;
  isLoading = false;
  error: string = '';
  successMessage: string = '';
  currentUser: any;

  accountTypes = [
    { value: 'Savings', label: 'Savings Account', description: 'Best for personal savings with interest' },
    { value: 'Current', label: 'Current Account', description: 'Ideal for business transactions' },
    { value: 'Fixed Deposit', label: 'Fixed Deposit', description: 'High interest for fixed tenure' }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private accountService: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.initializeForm();
  }

  private initializeForm(): void {
    this.openAccountForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      accountType: ['Savings', [Validators.required]],
      residentialAddress: ['', [Validators.required, Validators.minLength(10)]],
      permanentAddress: ['', [Validators.required, Validators.minLength(10)]],
      mobileNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
      email: ['', [Validators.required, Validators.email, Validators.pattern(/^[a-zA-Z0-9._%+-]+@gmail\.com$/)]],
      aadharCardNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{12}$/)]],
      occupationDetails: ['', [Validators.required, Validators.minLength(3)]],
      balance: [0, [Validators.required, Validators.min(0)]],
      enableNetBanking: [true]
    });
  }

  onSubmit(): void {
    if (this.openAccountForm.valid) {
      this.isLoading = true;
      this.error = '';
      this.successMessage = '';

      const userId = this.authService.getUserId();
      if (!userId) {
        this.error = 'User not logged in. Please login again.';
        this.isLoading = false;
        return;
      }

      const accountData: PendingAccountDTO = {
        userId: userId,
        name: this.openAccountForm.value.name,
        accountType: this.openAccountForm.value.accountType,
        residentialAddress: this.openAccountForm.value.residentialAddress,
        permanentAddress: this.openAccountForm.value.permanentAddress,
        mobileNumber: this.openAccountForm.value.mobileNumber,
        email: this.openAccountForm.value.email,
        aadharCardNumber: this.openAccountForm.value.aadharCardNumber,
        occupationDetails: this.openAccountForm.value.occupationDetails,
        balance: this.openAccountForm.value.balance,
        enableNetBanking: this.openAccountForm.value.enableNetBanking
      };

      this.accountService.openAccount(accountData).subscribe({
        next: (response) => {
          this.isLoading = false;
          if (response.success) {
            this.successMessage = response.message || 'Account opening request submitted successfully! Please wait for admin approval.';
            this.error = '';
            
            setTimeout(() => {
              this.router.navigate(['/userDashboard']);
            }, 3000);
          } else {
            this.error = response.message || 'Failed to submit account opening request. Please try again.';
            this.successMessage = '';
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.error = err?.error?.message || 'Failed to submit account opening request. Please try again.';
          this.successMessage = '';
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.openAccountForm.controls).forEach(key => {
      this.openAccountForm.get(key)?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.openAccountForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required`;
      }
      if (field.errors['minlength']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['email']) {
        return 'Please enter a valid email address';
      }
      if (field.errors['pattern']) {
        if (fieldName === 'mobileNumber') {
          return 'Mobile number must be exactly 10 digits';
        }
        if (fieldName === 'aadharCardNumber') {
          return 'Aadhaar number must be exactly 12 digits';
        }
        if (fieldName === 'email') {
          return 'Email must be a Gmail address';
        }
      }
      if (field.errors['min']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['min'].min}`;
      }
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      name: 'Full Name',
      accountType: 'Account Type',
      residentialAddress: 'Residential Address',
      permanentAddress: 'Permanent Address',
      mobileNumber: 'Mobile Number',
      email: 'Email',
      aadharCardNumber: 'Aadhaar Number',
      occupationDetails: 'Occupation Details',
      balance: 'Initial Balance'
    };
    return displayNames[fieldName] || fieldName;
  }

  goBack(): void {
    this.router.navigate(['/userDashboard']);
  }

  getMaxDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  getAccountTypeDescription(): string {
    const selectedType = this.openAccountForm.get('accountType')?.value;
    const accountType = this.accountTypes.find(t => t.value === selectedType);
    return accountType?.description || '';
  }
}