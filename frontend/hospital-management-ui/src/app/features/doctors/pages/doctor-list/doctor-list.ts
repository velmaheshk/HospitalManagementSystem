import { Component, OnInit, inject } from '@angular/core';
import {
  Router,
  RouterLink
} from '@angular/router';

import { DoctorApiService } from '../../../../core/services/doctor-api.service';
import { Doctor } from '../../../../core/models/doctor.model';

@Component({
  selector: 'app-doctor-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './doctor-list.html',
  styleUrl: './doctor-list.scss',
})
export class DoctorListComponent implements OnInit {

  private readonly doctorService = inject(DoctorApiService);
  private readonly router = inject(Router);

  doctors: Doctor[] = [];
  filteredDoctors: Doctor[] = [];

  searchText = '';
  loading = false;

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors(): void {
    this.loading = true;

    this.doctorService.getAll().subscribe({
      next: (response) => {
        this.doctors = response;
        this.filteredDoctors = response;
        this.loading = false;
      },

      error: (error) => {
        console.error('Failed to load doctors', error);
        this.loading = false;
      }
    });
  }

  searchDoctors(event: Event): void {

    const input = event.target as HTMLInputElement;

    this.searchText = input.value.trim().toLowerCase();

    const searchTerm = this.searchText;

    this.filteredDoctors = this.doctors.filter((doctor) => {

      const firstName =
        doctor.firstName?.toLowerCase() ?? '';

      const lastName =
        doctor.lastName?.toLowerCase() ?? '';

      const specialization =
        doctor.specialization?.toLowerCase() ?? '';

      const phone =
        doctor.phoneNumber?.toLowerCase() ?? '';

      return (
        firstName.includes(searchTerm) ||
        lastName.includes(searchTerm) ||
        specialization.includes(searchTerm) ||
        phone.includes(searchTerm)
      );
    });
  }

  editDoctor(id: number): void {

    this.router.navigate([
      '/doctor/edit',
      id
    ]);
  }

  deleteDoctor(doctor: Doctor): void {

    // Doctor.id is optional, so check it first.
    if (doctor.id == null) {
      console.error('Cannot delete doctor: Doctor ID is missing.');
      return;
    }

    const confirmed = confirm(
      `Are you sure you want to delete Dr. ${doctor.firstName} ${doctor.lastName}?`
    );

    if (!confirmed) {
      return;
    }

    this.doctorService.delete(doctor.id).subscribe({

      next: () => {
        this.loadDoctors();
      },

      error: (error) => {
        console.error(
          'Delete failed',
          error
        );
      }
    });
  }
}