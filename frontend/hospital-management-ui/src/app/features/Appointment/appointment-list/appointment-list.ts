import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import {
  AppointmentResponseDto,
  AppointmentStatus,
  AppointmentService,
  getStatusLabel
} from '../../../Service/appointment';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './appointment-list.html',
  styleUrl: './appointment-list.scss'
})
export class AppointmentList implements OnInit {

  appointments: AppointmentResponseDto[] = [];

  loading = false;
  errorMessage = '';

  constructor(
    private appointmentService: AppointmentService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {

    this.loading = true;
    this.errorMessage = '';

    console.log('🔵 API call started');

    this.appointmentService.getAllAppointments().subscribe({

      next: (response: any) => {

        console.log('🟢 RAW RESPONSE:', response);
        console.log('🟢 Is Array:', Array.isArray(response));

        // Defensive handling — backend sometimes wraps array in an object
        if (Array.isArray(response)) {
          this.appointments = response;
        } else if (response?.data && Array.isArray(response.data)) {
          this.appointments = response.data;
        } else if (response?.result && Array.isArray(response.result)) {
          this.appointments = response.result;
        } else if (response?.appointments && Array.isArray(response.appointments)) {
          this.appointments = response.appointments;
        } else {
          console.error('🔴 Unexpected response shape, could not extract array:', response);
          this.appointments = [];
        }

        this.loading = false;
        this.cdr.detectChanges();

        console.log('🟢 appointments.length:', this.appointments.length);
      },

      error: (error) => {

        console.error('🔴 Error loading appointments:', error);

        this.errorMessage =
          error?.error?.message ||
          'Failed to load appointments.';

        this.loading = false;
        this.cdr.detectChanges();
      }

    });
  }

  bookAppointment(): void {
    this.router.navigate(['/appointments/book']);
  }

  viewDetails(id: number): void {
    this.router.navigate(['/appointments', id]);
  }

  editAppointment(id: number): void {
    this.router.navigate(['/appointments/edit', id]);
  }

  cancelAppointment(id: number): void {

    const confirmed =
      confirm('Are you sure you want to cancel this appointment?');

    if (!confirmed) {
      return;
    }

    this.appointmentService.cancelAppointment(id).subscribe({

      next: () => {

        alert('Appointment cancelled successfully.');

        this.loadAppointments();
      },

      error: (error) => {

        console.error(
          'Error cancelling appointment:',
          error
        );

        alert(
          error?.error?.message ||
          'Failed to cancel appointment.'
        );
      }

    });
  }

  completeAppointment(id: number): void {

    const confirmed =
      confirm('Mark this appointment as completed?');

    if (!confirmed) {
      return;
    }

    this.appointmentService.completeAppointment(id).subscribe({

      next: () => {

        alert('Appointment completed successfully.');

        this.loadAppointments();
      },

      error: (error) => {

        console.error(
          'Error completing appointment:',
          error
        );

        alert(
          error?.error?.message ||
          'Failed to complete appointment.'
        );
      }

    });
  }

  getStatusClass(status: AppointmentStatus): string {

    switch (status) {

      case AppointmentStatus.Scheduled:
        return 'scheduled';

      case AppointmentStatus.Completed:
        return 'completed';

      case AppointmentStatus.Cancelled:
        return 'cancelled';

      case AppointmentStatus.NoShow:
        return 'noshow';

      default:
        return 'default';
    }
  }

  getStatusLabel(status: AppointmentStatus): string {
    return getStatusLabel(status);
  }

  isScheduled(status: AppointmentStatus): boolean {
    return status === AppointmentStatus.Scheduled;
  }
}