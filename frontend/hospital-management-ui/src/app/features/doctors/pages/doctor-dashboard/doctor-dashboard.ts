import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DoctorApiService } from '../../../../core/services/doctor-api.service';
import { doctordashboard } from '../../../../core/models/doctordashboard.model';

interface RecentDoctorRow {
  id: number;
  fullName: string;
  specialization: string;
  department: string;
  phone: string;
  status: string;
}

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.scss',
})
export class DoctorDashboardComponent implements OnInit {
  private readonly doctorService = inject(DoctorApiService);
  private readonly cdr = inject(ChangeDetectorRef);

  dashboard: doctordashboard | null = null;
  recentDoctors: RecentDoctorRow[] = [];
  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;
    this.errorMessage = '';

    this.doctorService.getDashboard().subscribe({
      next: (response) => {
        this.dashboard = response;
        this.loading = false;
        this.loadRecentDoctors();
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load doctors. Please try again.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadRecentDoctors(): void {
    this.doctorService.getDoctors().subscribe({
      next: (doctors) => {
        this.recentDoctors = doctors
          .slice(0, 5)
          .map((doctor) => ({
            id: doctor.doctorId,
            fullName: doctor.fullName || 'Unknown Doctor',
            specialization: doctor.specialization || 'General',
            department: doctor.departmentName || 'General Medicine',
            phone: doctor.phone ?? 'N/A',
            status: doctor.isActive ? 'Active' : 'Inactive',
          }));
        this.cdr.detectChanges();
      },
      error: () => {
        this.recentDoctors = [];
        this.cdr.detectChanges();
      },
    });
  }
}