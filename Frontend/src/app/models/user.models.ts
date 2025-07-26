export interface User {
  authId: number;
  username: string;
  role: 'Admin' | 'User';
  mobileNumber: string;
}

export interface AuthDTO {
  username: string;
  password: string;
  role: 'Admin' | 'User';
  mobileNumber: string;
}

export interface LoginDTO {
  username: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  data: {
    token: string;
    authId: number;
  };
}

export interface RegisterResponse {
  success: boolean;
  message: string;
  data: {
    authId: number;
  };
}