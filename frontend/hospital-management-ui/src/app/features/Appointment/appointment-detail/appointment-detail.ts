import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import {
  AppointmentService,
  AppointmentResponseDto,
  AppointmentStatus,
  getStatusLabel
} from '../../../Service/appointment';

@Component({
  selector: 'app-appointment-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './appointment-detail.html',
  styleUrl: './appointment-detail.scss',
})
export class AppointmentDetail implements OnInit {

  appointment: AppointmentResponseDto | null = null;
  loading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadAppointmentDetails(+id);
    } else {
      this.errorMessage = 'Invalid appointment ID.';
    }
  }

  loadAppointmentDetails(id: number): void {
    this.loading = true;
    this.errorMessage = '';

    this.appointmentService.getAppointmentById(id).subscribe({
      next: (data) => {
        this.appointment = data;
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load appointment details.';
        this.loading = false;
      }
    });
  }

  getStatusLabel(status: AppointmentStatus): string {
    return getStatusLabel(status);
  }

  getStatusClass(status: AppointmentStatus): string {
    switch (status) {
      case AppointmentStatus.Scheduled: return 'status-scheduled';
      case AppointmentStatus.Completed: return 'status-completed';
      case AppointmentStatus.Cancelled: return 'status-cancelled';
      case AppointmentStatus.NoShow: return 'status-noshow';
      default: return '';
    }
  }

  isScheduled(status: AppointmentStatus): boolean {
    return status === AppointmentStatus.Scheduled;
  }

  editAppointment(id: number): void {
    this.router.navigate(['/appointments/edit', id]);
  }

  completeAppointment(id: number): void {
    if (!this.appointment) return;

    this.appointmentService.completeAppointment(id).subscribe({
      next: () => this.loadAppointmentDetails(id),
      error: () => this.errorMessage = 'Failed to mark appointment as completed.'
    });
  }

  cancelAppointment(id: number): void {
    if (!this.appointment) return;

    this.appointmentService.cancelAppointment(id).subscribe({
      next: () => this.loadAppointmentDetails(id),
      error: () => this.errorMessage = 'Failed to cancel appointment.'
    });
  }

  goBack(): void {
    this.router.navigate(['/appointments']);
  }
}