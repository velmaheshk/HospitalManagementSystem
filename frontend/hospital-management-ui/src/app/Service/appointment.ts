import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// =====================================================
// ENUM
// =====================================================
export enum AppointmentStatus {
  Scheduled = 1,
  Completed = 2,
  Cancelled = 3,
  NoShow = 4
}

// =====================================================
// DTOs
// =====================================================
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

export interface UpdateAppointmentDto {
  appointmentDate: string;
  timeSlot: string;
  reason?: string | null;
}

// =====================================================
// HELPERS
// =====================================================
export function getStatusLabel(status: AppointmentStatus): string {
  switch (status) {
    case AppointmentStatus.Scheduled: return 'Scheduled';
    case AppointmentStatus.Completed: return 'Completed';
    case AppointmentStatus.Cancelled: return 'Cancelled';
    case AppointmentStatus.NoShow: return 'No Show';
    default: return 'Unknown';
  }
}

// =====================================================
// SERVICE
// =====================================================
@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private apiUrl = 'https://localhost:44343/api/Appointment';

  constructor(private http: HttpClient) {}

  // POST: api/Appointment
  createAppointment(
    dto: CreateAppointmentDto
  ): Observable<AppointmentResponseDto> {

    return this.http.post<AppointmentResponseDto>(
      this.apiUrl,
      dto
    );
  }

  // GET: api/Appointment
  getAllAppointments(): Observable<AppointmentResponseDto[]> {

    return this.http.get<AppointmentResponseDto[]>(
      this.apiUrl
    );
  }

  // GET: api/Appointment/1
  getAppointmentById(
    id: number
  ): Observable<AppointmentResponseDto> {

    return this.http.get<AppointmentResponseDto>(
      `${this.apiUrl}/${id}`
    );
  }

  // GET: api/Appointment/patient/1
  getAppointmentsByPatient(
    patientId: number
  ): Observable<AppointmentResponseDto[]> {

    return this.http.get<AppointmentResponseDto[]>(
      `${this.apiUrl}/patient/${patientId}`
    );
  }

  // GET: api/Appointment/doctor/1
  getAppointmentsByDoctor(
    doctorId: number
  ): Observable<AppointmentResponseDto[]> {

    return this.http.get<AppointmentResponseDto[]>(
      `${this.apiUrl}/doctor/${doctorId}`
    );
  }

  // GET: api/Appointment/doctor/1/date/2026-08-15
  getAppointmentsByDoctorAndDate(
    doctorId: number,
    date: string
  ): Observable<AppointmentResponseDto[]> {

    return this.http.get<AppointmentResponseDto[]>(
      `${this.apiUrl}/doctor/${doctorId}/date/${date}`
    );
  }

  // PUT: api/Appointment/1
  updateAppointment(
    id: number,
    dto: UpdateAppointmentDto
  ): Observable<AppointmentResponseDto> {

    return this.http.put<AppointmentResponseDto>(
      `${this.apiUrl}/${id}`,
      dto
    );
  }

  // PATCH: api/Appointment/1/cancel
  cancelAppointment(
    id: number
  ): Observable<any> {

    return this.http.patch(
      `${this.apiUrl}/${id}/cancel`,
      {}
    );
  }

  // PATCH: api/Appointment/1/complete
  completeAppointment(
    id: number
  ): Observable<any> {

    return this.http.patch(
      `${this.apiUrl}/${id}/complete`,
      {}
    );
  }
}