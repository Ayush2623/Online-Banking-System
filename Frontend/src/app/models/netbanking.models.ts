export interface NetBanking {
  netBankingId: number;
  accountNumber: string;
  username: string;
  isActive: boolean;
  registrationDate: string;
  lastLoginDate?: string;
}

export interface NetBankingDTO {
  AccountNumber: number;
  Username: string;
  Password: string;
}

export interface NetBankingRegistrationDTO {
  accountNumber: string;
  loginPassword: string;
  transactionPassword: string;
}

export interface NetBankingUser {
  netBankingId: number;
  accountNumber: string;
  username: string;
  isActive: boolean;
  registrationDate: string;
  lastLoginDate?: string;
}

export interface EnableNetBankingRequest {
  accountNumber: string;
  username: string;
  password: string;
  mobileNumber: string;
  email: string;
}

export interface UpdateNetBankingPasswordRequest {
  accountId: number;
  oldPassword: string;
  newPassword: string;
}

export interface NetBankingDetails {
  AccountNumber: number;
  Username: string;
  CreatedAt: string;
}