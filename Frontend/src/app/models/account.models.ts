export interface Account {
  accountId: number;
  accountNumber: string;
  accountHolderName: string;
  balance: number;
  accountType: string;
  branchCode: string;
  ifscCode: string;
  address: string;
  mobileNumber: string;
  email: string;
  aadhaarNumber: string;
  panNumber: string;
  dateOfBirth: string;
  isActive: boolean;
  createdDate: string;
  lastModifiedDate: string;
}

export interface PendingAccountDTO {
  userId: number;
  name: string; // Backend expects 'Name'
  accountType: string;
  residentialAddress: string; // Backend expects 'ResidentialAddress'
  permanentAddress: string; // Backend expects 'PermanentAddress'
  mobileNumber: string;
  email: string;
  aadharCardNumber: string; // Backend expects 'AadharCardNumber'
  occupationDetails: string; // Backend expects 'OccupationDetails'
  balance: number; // Backend expects 'Balance'
  enableNetBanking: boolean;
}

export interface PendingAccount {
  requestId: number;
  userId: number;
  name: string; // Backend returns 'name' not 'accountHolderName'
  accountType: string;
  residentialAddress: string;
  permanentAddress: string;
  mobileNumber: string;
  email: string;
  aadharCardNumber: string; // Backend returns 'aadharCardNumber' not 'aadhaarNumber'
  occupationDetails: string;
  balance: number;
  isNetBankingEnabled: boolean; // Backend returns 'isNetBankingEnabled' not 'enableNetBanking'
  createdAt: string; // Backend returns 'createdAt' not 'requestDate'
  status: 'Pending' | 'Approved' | 'Rejected';
}

export interface UpdateAccountDTO {
  accountHolderName?: string;
  address?: string;
  mobileNumber?: string;
  email?: string;
}

export interface ForgotPasswordRequest {
  userId: number;
  mobileNumber: string;
  email: string;
}

export interface ForgotUserIdRequest {
  userId: number;
  mobileNumber: string;
  email: string;
}

export interface SetNewPasswordRequest {
  userId: number;
  newPassword: string;
  resetToken: string;
}