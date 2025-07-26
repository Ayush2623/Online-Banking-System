export interface Payee {
  payeeId: number;
  payeeName: string;
  payeeAccountNumber: string;
  nickname: string;
  accountNumber: string;
}

export interface PayeeDTO {
  payeeName: string;
  payeeAccountNumber: string;
  nickname: string;
  accountNumber: string;
}

export interface FundTransferRequest {
  fromAccountNumber: string;
  toAccountNumber: string;
  amount: number;
  remarks: string;
  transferMode: 'IMPS' | 'NEFT' | 'RTGS';
}