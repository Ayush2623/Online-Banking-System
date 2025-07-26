import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { AccountService } from '../../../../services/account.service';
import { FundTransferService } from '../../../../services/fund-transfer.service';
import { Account } from '../../../../models/account.models';
import { PayeeDTO, Payee } from '../../../../models/fund-transfer.models';

@Component({
  selector: 'app-payee-management',
  templateUrl: './payee-management.component.html',
  styleUrls: ['./payee-management.component.scss']
})
export class PayeeManagementComponent implements OnInit {
  addPayeeForm!: FormGroup;
  isLoading = false;
  error: string = '';
  successMessage: string = '';
  currentUser: any;
  userAccount: Account | null = null;
  payees: Payee[] = [];
  showAddForm = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private accountService: AccountService,
    private fundTransferService: FundTransferService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.initializeForm();
    this.loadUserAccount();
  }

  private initializeForm(): void {
    this.addPayeeForm = this.formBuilder.group({
      payeeName: ['', [Validators.required, Validators.minLength(2)]],
      payeeAccountNumber: ['', [Validators.required, Validators.pattern(/^[0-9]{9,18}$/)]],
      confirmAccountNumber: ['', [Validators.required]],
      nickname: ['', [Validators.minLength(2)]]
    }, { validators: this.accountNumberMatchValidator });
  }

  private accountNumberMatchValidator(form: FormGroup) {
    const accountNumber = form.get('payeeAccountNumber');
    const confirmAccountNumber = form.get('confirmAccountNumber');
    
    if (accountNumber && confirmAccountNumber && accountNumber.value !== confirmAccountNumber.value) {
      confirmAccountNumber.setErrors({ accountNumberMismatch: true });
    } else if (confirmAccountNumber?.hasError('accountNumberMismatch')) {
      confirmAccountNumber.setErrors(null);
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
        this.loadPayees();
      },
      error: (err) => {
        this.error = 'Failed to load account information.';
        console.error('Error loading account:', err);
      }
    });
  }

  private loadPayees(): void {
    if (this.userAccount) {
      this.fundTransferService.getPayees(this.userAccount.accountNumber).subscribe({
        next: (payees) => {
          this.payees = payees;
        },
        error: (err) => {
          console.error('Error loading payees:', err);
          this.payees = [];
        }
      });
    }
  }

  toggleAddForm(): void {
    this.showAddForm = !this.showAddForm;
    if (!this.showAddForm) {
      this.addPayeeForm.reset();
      this.error = '';
      this.successMessage = '';
    }
  }

  onSubmit(): void {
    if (this.addPayeeForm.valid && this.userAccount) {
      this.isLoading = true;
      this.error = '';
      this.successMessage = '';

      const payeeData: PayeeDTO = {
        payeeName: this.addPayeeForm.value.payeeName,
        payeeAccountNumber: this.addPayeeForm.value.payeeAccountNumber,
        nickname: this.addPayeeForm.value.nickname || '',
        accountNumber: this.userAccount.accountNumber
      };

      this.fundTransferService.addPayee(payeeData).subscribe({
        next: (response) => {
          this.isLoading = false;
          if (response.success) {
            this.successMessage = response.message || 'Payee added successfully!';
            this.addPayeeForm.reset();
            this.showAddForm = false;
            this.loadPayees(); // Reload payees list
            this.error = '';
          } else {
            this.error = response.message || 'Failed to add payee. Please try again.';
            this.successMessage = '';
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.error = err?.error?.message || 'Failed to add payee. Please try again.';
          this.successMessage = '';
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.addPayeeForm.controls).forEach(key => {
      this.addPayeeForm.get(key)?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.addPayeeForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required`;
      }
      if (field.errors['minlength']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['pattern']) {
        if (fieldName === 'payeeAccountNumber') {
          return 'Account number must be 9-18 digits';
        }
      }
      if (field.errors['accountNumberMismatch']) {
        return 'Account numbers do not match';
      }
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      payeeName: 'Payee Name',
      payeeAccountNumber: 'Account Number',
      confirmAccountNumber: 'Confirm Account Number',
      nickname: 'Nickname'
    };
    return displayNames[fieldName] || fieldName;
  }

  goToFundTransfer(): void {
    this.router.navigate(['/userDashboard/fund-transfer']);
  }

  goBack(): void {
    this.router.navigate(['/userDashboard']);
  }

  deletePayee(payeeId: number): void {
    if (confirm('Are you sure you want to delete this payee?')) {
      // Note: This would need to be implemented in the backend
      console.log('Delete payee:', payeeId);
    }
  }

  editPayee(payee: Payee): void {
    // Note: This would need to be implemented
    console.log('Edit payee:', payee);
  }
}