import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { AccountService } from '../../../../services/account.service';
import { FundTransferService } from '../../../../services/fund-transfer.service';
import { Account } from '../../../../models/account.models';
import { FundTransferRequest, Payee } from '../../../../models/fund-transfer.models';

@Component({
  selector: 'app-fund-transfer',
  templateUrl: './fund-transfer.component.html',
  styleUrls: ['./fund-transfer.component.scss']
})
export class FundTransferComponent implements OnInit {
  fundTransferForm!: FormGroup;
  isLoading = false;
  error: string = '';
  successMessage: string = '';
  currentUser: any;
  userAccount: Account | null = null;
  payees: Payee[] = [];
  selectedPayee: Payee | null = null;
  showPayeeForm = false;

  transactionTypes = [
    { value: 'IMPS', label: 'IMPS (Immediate)', description: 'Instant transfer 24x7' },
    { value: 'NEFT', label: 'NEFT (National)', description: 'Batch processing during banking hours' },
    { value: 'RTGS', label: 'RTGS (Real Time)', description: 'Real-time gross settlement for large amounts' }
  ];

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
    this.fundTransferForm = this.formBuilder.group({
      payeeSelection: ['', [Validators.required]],
      amount: ['', [Validators.required, Validators.min(1), Validators.max(100000)]],
      description: ['', [Validators.required, Validators.minLength(3)]],
      transactionType: ['IMPS', [Validators.required]]
    });
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

  onPayeeSelect(): void {
    const payeeId = this.fundTransferForm.get('payeeSelection')?.value;
    if (payeeId === 'new') {
      this.showPayeeForm = true;
      this.selectedPayee = null;
    } else {
      this.selectedPayee = this.payees.find(p => p.payeeId == payeeId) || null;
      this.showPayeeForm = false;
    }
  }

  onSubmit(): void {
    if (this.fundTransferForm.valid && this.userAccount) {
      if (this.fundTransferForm.get('payeeSelection')?.value === 'new' && !this.selectedPayee) {
        this.error = 'Please add a payee first or select an existing one.';
        return;
      }

      this.isLoading = true;
      this.error = '';
      this.successMessage = '';

      const toAccountNumber = this.selectedPayee?.payeeAccountNumber || '';
      
      const transferRequest: FundTransferRequest = {
        fromAccountNumber: this.userAccount.accountNumber,
        toAccountNumber: toAccountNumber,
        amount: this.fundTransferForm.value.amount,
        remarks: this.fundTransferForm.value.description,
        transferMode: this.fundTransferForm.value.transactionType
      };

      this.fundTransferService.transferFunds(transferRequest).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.successMessage = 'Fund transfer completed successfully!';
          this.fundTransferForm.reset();
          this.selectedPayee = null;
          
          setTimeout(() => {
            this.router.navigate(['/userDashboard']);
          }, 3000);
        },
        error: (err) => {
          this.isLoading = false;
          this.error = err?.error?.message || 'Failed to transfer funds. Please try again.';
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.fundTransferForm.controls).forEach(key => {
      this.fundTransferForm.get(key)?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.fundTransferForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required`;
      }
      if (field.errors['min']) {
        return `Amount must be at least ₹${field.errors['min'].min}`;
      }
      if (field.errors['max']) {
        return `Amount cannot exceed ₹${field.errors['max'].max}`;
      }
      if (field.errors['minlength']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      payeeSelection: 'Payee',
      amount: 'Amount',
      description: 'Description',
      transactionType: 'Transaction Type'
    };
    return displayNames[fieldName] || fieldName;
  }

  goToPayeeManagement(): void {
    this.router.navigate(['/userDashboard/payee-management']);
  }

  goBack(): void {
    this.router.navigate(['/userDashboard']);
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(amount);
  }

  getTransactionTypeDescription(): string {
    const selectedType = this.fundTransferForm.get('transactionType')?.value;
    const transactionType = this.transactionTypes.find(t => t.value === selectedType);
    return transactionType?.description || '';
  }
}