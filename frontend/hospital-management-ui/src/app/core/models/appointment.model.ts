export enum AppointmentStatus {
  Scheduled = 1,
  Completed = 2,
  Cancelled = 3,
  NoShow = 4
}

export interface AppointmentResponseDto {
  appointmentId: number;

  patientId: number;
  patientName: string;

  doctorId: number;
  doctorName: string;

  appointmentDate: string;
  timeSlot: string;

  status: AppointmentStatus;

  reason: string | null;

  createdAt: string;
}

export interface CreateAppointmentDto {
  patientId: number;
  doctorId: number;
  appointmentDate: string;
  timeSlot: string;
  reason?: string | null;
}


export function getStatusLabel(status: AppointmentStatus): string {
  switch (status) {
    case AppointmentStatus.Scheduled: return 'Scheduled';
    case AppointmentStatus.Completed: return 'Completed';
    case AppointmentStatus.Cancelled: return 'Cancelled';
    case AppointmentStatus.NoShow: return 'No Show';
    default: return 'Unknown';
  }
}