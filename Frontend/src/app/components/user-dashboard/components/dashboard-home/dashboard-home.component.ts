import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { AccountService } from '../../../../services/account.service';
import { DashboardService } from '../../../../services/dashboard.service';
import { Account } from '../../../../models/account.models';
import { DashboardData, Transaction } from '../../../../models/dashboard.models';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent implements OnInit {
  currentUser: any;
  accountDetails: Account | null = null;
  dashboardData: DashboardData | null = null;
  recentTransactions: Transaction[] = [];
  isLoading: boolean = false;
  error: string = '';
  hasAccount: boolean = false;

  constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private dashboardService: DashboardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.checkUserAccount();
  }

  private checkUserAccount(): void {
    const userId = this.authService.getUserId();
    
    if (!userId) {
      this.router.navigate(['/login']);
      return;
    }

    this.isLoading = true;
    this.accountService.viewAccountByUserId(userId).subscribe({
      next: (account) => {
        this.accountDetails = account;
        this.hasAccount = true;
        this.loadDashboardData(account.accountNumber);
      },
      error: (err) => {
        this.isLoading = false;
        console.log('Account loading error:', err);
        if (err.status === 404 || err.status === 403 || 
            (err.error?.message && err.error.message.includes('Account not found'))) {
          // User doesn't have an account yet
          this.hasAccount = false;
        } else {
          this.error = err?.error?.message || 'Failed to load account information. Please try again.';
        }
      }
    });
  }

  private loadDashboardData(accountNumber: string): void {
    // Load dashboard data (user profile)
    this.dashboardService.getDashboard(accountNumber).subscribe({
      next: (data) => {
        this.dashboardData = data;
      },
      error: (err) => {
        console.error('Error loading dashboard data:', err);
      }
    });

    // Load account summary (balance + recent transactions)
    this.dashboardService.getAccountSummary(accountNumber).subscribe({
      next: (summary) => {
        this.isLoading = false;
        this.recentTransactions = summary.recentTransactions || [];
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error loading account summary:', err);
      }
    });
  }

  openAccount(): void {
    this.router.navigate(['/userDashboard/open-account']);
  }

  viewAccountStatement(): void {
    this.router.navigate(['/userDashboard/account-statement']);
  }

  transferFunds(): void {
    this.router.navigate(['/userDashboard/fund-transfer']);
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

  refreshData(): void {
    this.checkUserAccount();
  }
}