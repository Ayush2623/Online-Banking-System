import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { AccountService } from '../../../../services/account.service';
import { DashboardService } from '../../../../services/dashboard.service';
import { Account } from '../../../../models/account.models';
import { Transaction } from '../../../../models/dashboard.models';

@Component({
  selector: 'app-account-statement',
  templateUrl: './account-statement.component.html',
  styleUrls: ['./account-statement.component.scss']
})
export class AccountStatementComponent implements OnInit {
  filterForm!: FormGroup;
  isLoading = false;
  error: string = '';
  currentUser: any;
  userAccount: Account | null = null;
  transactions: Transaction[] = [];
  filteredTransactions: Transaction[] = [];
  totalTransactions: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  
  filterOptions = {
    types: [
      { value: 'all', label: 'All Transactions' },
      { value: 'Credit', label: 'Credit Only' },
      { value: 'Debit', label: 'Debit Only' }
    ]
  };

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private accountService: AccountService,
    private dashboardService: DashboardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.initializeForm();
    this.loadUserAccount();
  }

  private initializeForm(): void {
    this.filterForm = this.formBuilder.group({
      transactionType: ['all', [Validators.required]],
      minAmount: [''],
      maxAmount: ['']
    });
  }


  private loadUserAccount(): void {
    const userId = this.authService.getUserId();
    if (!userId) {
      this.router.navigate(['/login']);
      return;
    }

    this.isLoading = true;
    this.accountService.viewAccountByUserId(userId).subscribe({
      next: (account) => {
        this.userAccount = account;
        this.loadTransactions();
      },
      error: (err) => {
        this.isLoading = false;
        this.error = 'Failed to load account information.';
        console.error('Error loading account:', err);
      }
    });
  }

  private loadTransactions(): void {
    if (!this.userAccount) return;

    console.log('Loading all transactions for account:', this.userAccount.accountNumber);

    this.isLoading = true;
    this.error = '';
    
    // Use getAllTransactions for complete transaction history (better for Account Statement)
    this.dashboardService.getAllTransactions(this.userAccount.accountNumber).subscribe({
      next: (transactions) => {
        console.log('Received transactions:', transactions);
        this.isLoading = false;
        
        // Convert Transaction model to display format and calculate balance
        this.transactions = transactions.map((t, index) => ({
          ...t,
          balanceAfterTransaction: this.userAccount!.balance, // We'd need to calculate this properly
          transactionType: this.determineTransactionType(t, this.userAccount!.accountNumber)
        }));
        
        this.applyFilters();
      },
      error: (err) => {
        console.error('Error loading transactions:', err);
        this.isLoading = false;
        this.error = err?.error?.message || 'Failed to load transaction history.';
      }
    });
  }

  private determineTransactionType(transaction: Transaction, userAccountNumber: string): 'Credit' | 'Debit' {
    // If money came TO this account, it's Credit
    // If money went FROM this account, it's Debit
    return transaction.toAccountNumber.toString() === userAccountNumber ? 'Credit' : 'Debit';
  }

  onFilterSubmit(): void {
    this.applyFilters();
  }

  private applyFilters(): void {
    let filtered = [...this.transactions];
    const formValue = this.filterForm.value;

    // Filter by transaction type
    if (formValue.transactionType && formValue.transactionType !== 'all') {
      filtered = filtered.filter(transaction => 
        transaction.transactionType === formValue.transactionType
      );
    }

    // Filter by amount range
    if (formValue.minAmount) {
      filtered = filtered.filter(transaction => 
        transaction.amount >= parseFloat(formValue.minAmount)
      );
    }
    if (formValue.maxAmount) {
      filtered = filtered.filter(transaction => 
        transaction.amount <= parseFloat(formValue.maxAmount)
      );
    }

    this.filteredTransactions = filtered;
    this.totalTransactions = filtered.length;
    this.currentPage = 1; // Reset to first page when filtering
  }

  get paginatedTransactions(): Transaction[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredTransactions.slice(startIndex, endIndex);
  }

  get totalPages(): number {
    return Math.ceil(this.totalTransactions / this.itemsPerPage);
  }

  get pageNumbers(): number[] {
    const total = this.totalPages;
    const current = this.currentPage;
    const pages: number[] = [];
    
    // Show page numbers around current page
    const start = Math.max(1, current - 2);
    const end = Math.min(total, current + 2);
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    
    return pages;
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  exportStatement(): void {
    // This would typically call a backend service to generate PDF/Excel
    const dataStr = JSON.stringify(this.filteredTransactions, null, 2);
    const dataUri = 'data:application/json;charset=utf-8,'+ encodeURIComponent(dataStr);
    
    const exportFileDefaultName = `account_statement_${this.userAccount?.accountNumber}_${new Date().toISOString().split('T')[0]}.json`;
    
    const linkElement = document.createElement('a');
    linkElement.setAttribute('href', dataUri);
    linkElement.setAttribute('download', exportFileDefaultName);
    linkElement.click();
  }

  printStatement(): void {
    window.print();
  }

  resetFilters(): void {
    this.initializeForm();
    this.applyFilters();
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(amount);
  }

  formatDate(dateString: string): string {
    try {
      return new Date(dateString).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (error) {
      return dateString;
    }
  }

  getTransactionTypeClass(type: string): string {
    return type === 'Credit' ? 'text-green-600' : 'text-red-600';
  }

  getTransactionIcon(type: string): string {
    return type === 'Credit' 
      ? 'M12 6v6m0 0v6m0-6h6m-6 0H6' 
      : 'M20 12H4';
  }

  goBack(): void {
    this.router.navigate(['/userDashboard']);
  }

  getMathMin(a: number, b: number): number {
    return Math.min(a, b);
  }
}