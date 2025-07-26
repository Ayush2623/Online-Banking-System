export interface DashboardData {
  name: string;
  email: string;
  mobileNumber: string;
  aadharCardNumber: string;
  dateOfBirth: string;
  residentialAddress: string;
  permanentAddress: string;
  occupation: string;
}

export interface Account {
  accountNumber: string;
  accountHolderName: string;
  balance: number;
  accountType: string;
  ifscCode: string;
  branchCode: string;
}

export interface Transaction {
  transactionId: number;
  fromAccountNumber: number;
  toAccountNumber: number;
  amount: number;
  transactionType: 'Credit' | 'Debit';
  remarks: string;
  transactionDate: string;
  balanceAfterTransaction?: number; // Optional for display
}

export interface AccountSummary {
  accountNumber: string;
  balance: number;
  recentTransactions: Transaction[];
}

export interface AccountStatement {
  transactions: Transaction[];
  startDate: string;
  endDate: string;
  openingBalance: number;
  closingBalance: number;
}

export interface ChangePasswordRequest {
  accountNumber: string;
  oldPassword: string;
  newPassword: string;
}