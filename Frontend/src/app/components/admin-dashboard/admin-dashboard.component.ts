import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AdminService } from '../../services/admin.service';
import { PendingAccount } from '../../models/account.models';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  pendingRequests: PendingAccount[] = [];
  isLoading = false;
  error: string = '';
  successMessage: string = '';
  currentUser: any;
  
  // Statistics
  totalPendingRequests = 0;
  totalApprovedToday = 0;
  totalRejectedToday = 0;

  constructor(
    private authService: AuthService,
    private adminService: AdminService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.checkAdminAccess();
    this.loadCurrentUser();
    this.loadPendingRequests();
  }

  private checkAdminAccess(): void {
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/login']);
      return;
    }
  }

  private loadCurrentUser(): void {
    this.currentUser = this.authService.getCurrentUser();
  }

  loadPendingRequests(): void {
    this.isLoading = true;
    this.error = '';

    this.adminService.getPendingRequests().subscribe({
      next: (requests) => {
        this.isLoading = false;
        this.pendingRequests = requests;
        this.updateStatistics();
      },
      error: (err) => {
        this.isLoading = false;
        this.error = 'Failed to load pending requests. Please try again.';
        console.error('Error loading pending requests:', err);
      }
    });
  }

  private updateStatistics(): void {
    this.totalPendingRequests = this.pendingRequests.filter(req => req.status === 'Pending').length;
    
    const today = new Date().toDateString();
    // Note: These would need to be calculated from backend data
    this.totalApprovedToday = 0;
    this.totalRejectedToday = 0;
  }

  approveAccount(requestId: number): void {
    if (confirm('Are you sure you want to approve this account request?')) {
      this.adminService.approveAccount(requestId).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage = response.message || 'Account approved successfully!';
            this.error = '';
            this.loadPendingRequests(); // Refresh the list
          } else {
            this.error = response.message || 'Failed to approve account. Please try again.';
            this.successMessage = '';
          }
          this.clearMessages();
        },
        error: (err) => {
          this.error = err?.error?.message || 'Failed to approve account. Please try again.';
          this.successMessage = '';
          this.clearMessages();
          console.error('Error approving account:', err);
        }
      });
    }
  }

  rejectAccount(requestId: number): void {
    if (confirm('Are you sure you want to reject this account request? This action cannot be undone.')) {
      this.adminService.rejectAccount(requestId).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage = response.message || 'Account rejected successfully!';
            this.error = '';
            this.loadPendingRequests(); // Refresh the list
          } else {
            this.error = response.message || 'Failed to reject account. Please try again.';
            this.successMessage = '';
          }
          this.clearMessages();
        },
        error: (err) => {
          this.error = err?.error?.message || 'Failed to reject account. Please try again.';
          this.successMessage = '';
          this.clearMessages();
          console.error('Error rejecting account:', err);
        }
      });
    }
  }

  private clearMessages(): void {
    setTimeout(() => {
      this.error = '';
      this.successMessage = '';
    }, 5000);
  }

  logout(): void {
    this.authService.logout();
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

  getStatusBadgeClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'pending':
        return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'approved':
        return 'bg-green-100 text-green-800 border-green-200';
      case 'rejected':
        return 'bg-red-100 text-red-800 border-red-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  }

  refreshData(): void {
    this.loadPendingRequests();
  }

  getCurrentDate(): string {
    return new Date().toLocaleDateString();
  }
}