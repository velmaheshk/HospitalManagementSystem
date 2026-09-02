export interface User {
  userId?: number;
  username: string;
  password?: string;
  email: string;
  phone?: string;
  role: string;
}

export interface CreateUserRequest {
  username: string;
  password: string;
  email: string;
  phone?: string;
  role: string;
}

export interface UpdateUserRequest {
  username: string;
  password?: string;
  email: string;
  phone?: string;
  role: string;
}