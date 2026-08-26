export interface CreateDoctorRequest {
  fullName: string;
  specialization: string;
  qualification: string;
  experienceYears: number;
  consultationFee: number;
  departmentId: number;
  username: string;
  password: string;
  email: string;
  phone: string;
}