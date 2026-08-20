export interface Doctor {
  id: number;

  firstName: string;
  lastName: string;

  fullName?: string;

  email: string;
  phoneNumber: string;

  specialization: string;

  qualification?: string;
  experienceYears?: number;

  consultationFee?: number;

  departmentId?: number;

  isActive: boolean;
}
