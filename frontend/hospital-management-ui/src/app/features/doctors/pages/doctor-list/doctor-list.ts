import { Component,OnInit,inject } from '@angular/core';
import {
  Router,
  RouterLink
} from '@angular/router';

import {
  DoctorApiService
} from '../../../../core/services/doctor-api.service';

import {
  Doctor
} from '../../../../core/models/doctor.model';

@Component({
  selector: 'app-doctor-list',
  standalone:true,
  imports: [RouterLink],
  templateUrl: './doctor-list.html',
  styleUrl: './doctor-list.scss',
})
export class DoctorListComponent implements OnInit{
private readonly doctorService =
    inject(DoctorApiService);

  private readonly router =
    inject(Router);

  doctors: Doctor[] = [];

  filteredDoctors: Doctor[] = [];

  searchText = '';

  loading = false;

  ngOnInit(): void {

    this.loadDoctors();

  }

  loadDoctors(): void {

    this.loading = true;

    this.doctorService
      .getAll()
      .subscribe({

        next: (response) => {

          this.doctors = response;

          this.filteredDoctors = response;

          this.loading = false;
        },

        error: (error) => {

          console.error(error);

          this.loading = false;
        }

      });
  }

  searchDoctors(event: Event): void {

    const value =
      (event.target as HTMLInputElement)
        .value
        .toLowerCase();

    this.searchText = value;

    this.filteredDoctors =
      this.doctors.filter(doctor => {

        return (
          doctor.firstName
            .toLowerCase()
            .includes(value)

          ||

          doctor.lastName
            .toLowerCase()
            .includes(value)

          ||

          doctor.specialization
            .toLowerCase()
            .includes(value)

          ||

          doctor.email
            .toLowerCase()
            .includes(value)
        );

      });

  }

  editDoctor(id: number): void {

    this.router.navigate([
      '/doctor/edit',
      id
    ]);

  }

  deleteDoctor(
    doctor: Doctor
  ): void {

    const confirmed =
      confirm(
        `Are you sure you want to delete Dr. ${doctor.firstName} ${doctor.lastName}?`
      );

    if (!confirmed) {
      return;
    }

    this.doctorService
      .delete(doctor.id)
      .subscribe({

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
