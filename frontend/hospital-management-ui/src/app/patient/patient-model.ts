export interface Patient {

  patientId: number;

  userId: number;

  fullName: string;

  dob: string;

  gender: string;

  address: string;

  bloodGroup: string;

  emergencyContactName: string;

  emergencyContactPhone: string;
}

export interface User {
  userId: number;
  username: string;
  passwordHash: string;
  email: string;
  phone: string;
  roleId: number;
  role?: Role;
}

export interface Role {
  roleId: number;
  roleName: string;
}