import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
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
  standalone: true,

  imports: [
    RouterLink
  ],

  templateUrl: './doctor-list.html',
  styleUrl: './doctor-list.scss'
})
export class DoctorListComponent implements OnInit {

  private readonly doctorService =
    inject(DoctorApiService);


  // =====================================================
  // DATA
  // =====================================================

  doctors: Doctor[] = [];

  filteredDoctors: Doctor[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';

  deletingDoctorId: number | null = null;


  // =====================================================
  // SEARCH / FILTER
  // =====================================================

  searchText = '';

  statusFilter:
    'all' |
    'active' |
    'inactive' = 'all';


  // =====================================================
  // INITIALIZATION
  // =====================================================

  ngOnInit(): void {

    this.loadDoctors();

  }


  // =====================================================
  // LOAD DOCTORS
  // =====================================================

  loadDoctors(): void {

    this.loading = true;

    this.errorMessage = '';


    this.doctorService.getAll().subscribe({

      next: (response: Doctor[]) => {

        console.log(
          'Doctors API Response:',
          response
        );


        // Normalize API response

        this.doctors =
          (response ?? []).map((doctor) => ({

            ...doctor,

            isActive:
              doctor.isActive === true ||
              (doctor.isActive as any) === 1 ||
              (doctor.isActive as any) === 'true' ||
              (doctor.isActive as any) === 'True'

          }));


        this.applyFilters();

        this.loading = false;

      },


      error: (error) => {

        console.error(
          'Failed to load doctors:',
          error
        );


        this.doctors = [];

        this.filteredDoctors = [];


        this.errorMessage =
          'Unable to load doctors. Please try again.';


        this.loading = false;

      }

    });

  }


  // =====================================================
  // SEARCH DOCTORS
  // =====================================================

  searchDoctors(event: Event): void {

    const input =
      event.target as HTMLInputElement;


    this.searchText =
      input.value
        .trim()
        .toLowerCase();


    this.applyFilters();

  }


  // =====================================================
  // STATUS FILTER
  // =====================================================

  filterByStatus(event: Event): void {

    const select =
      event.target as HTMLSelectElement;


    this.statusFilter =
      select.value as
        | 'all'
        | 'active'
        | 'inactive';


    this.applyFilters();

  }


  // =====================================================
  // APPLY SEARCH + STATUS FILTER
  // =====================================================

  private applyFilters(): void {

    const search =
      this.searchText
        .trim()
        .toLowerCase();


    this.filteredDoctors =
      this.doctors.filter((doctor) => {


        // -----------------------------------------------
        // NAME
        // -----------------------------------------------

        const firstName =
          doctor.firstName
            ?.toLowerCase() ?? '';


        const lastName =
          doctor.lastName
            ?.toLowerCase() ?? '';


        const fullName =
          `${firstName} ${lastName}`.trim();


        // -----------------------------------------------
        // OTHER FIELDS
        // -----------------------------------------------

        const email =
          doctor.email
            ?.toLowerCase() ?? '';


        const phone =
          doctor.phoneNumber
            ?.toLowerCase() ?? '';


        const specialization =
          doctor.specialization
            ?.toLowerCase() ?? '';


        // -----------------------------------------------
        // SEARCH
        // -----------------------------------------------

        const matchesSearch =
          !search ||

          fullName.includes(search) ||

          firstName.includes(search) ||

          lastName.includes(search) ||

          email.includes(search) ||

          phone.includes(search) ||

          specialization.includes(search);


        // -----------------------------------------------
        // STATUS
        // -----------------------------------------------

        const matchesStatus =

          this.statusFilter === 'all'

          ||

          (
            this.statusFilter === 'active' &&
            doctor.isActive === true
          )

          ||

          (
            this.statusFilter === 'inactive' &&
            doctor.isActive === false
          );


        return (
          matchesSearch &&
          matchesStatus
        );

      });

  }


  // =====================================================
  // DOCTOR INITIALS
  // =====================================================

  getDoctorInitials(
    firstName?: string | null,
    lastName?: string | null
  ): string {

    const first =
      firstName?.trim().charAt(0) ?? '';


    const last =
      lastName?.trim().charAt(0) ?? '';


    const initials =
      `${first}${last}`.toUpperCase();


    return initials || 'DR';

  }


  // =====================================================
  // TOTAL DOCTORS
  // =====================================================

  get totalDoctorCount(): number {

    return this.doctors.length;

  }


  // =====================================================
  // ACTIVE DOCTORS
  // =====================================================

  get activeDoctorCount(): number {

    return this.doctors.filter(
      doctor => doctor.isActive === true
    ).length;

  }


  // =====================================================
  // INACTIVE DOCTORS
  // =====================================================

  get inactiveDoctorCount(): number {

    return this.doctors.filter(
      doctor => doctor.isActive === false
    ).length;

  }


  // =====================================================
  // DELETE DOCTOR
  // =====================================================

  deleteDoctor(doctor: Doctor): void {

    if (doctor.id == null) {

      console.error(
        'Cannot delete doctor: ID is missing.'
      );

      return;

    }


    const doctorName =
      `${doctor.firstName ?? ''} ${doctor.lastName ?? ''}`
        .trim();


    const confirmed =
      window.confirm(
        `Are you sure you want to delete Dr. ${doctorName}?`
      );


    if (!confirmed) {

      return;

    }


    this.deletingDoctorId =
      doctor.id;


    this.doctorService
      .delete(doctor.id)
      .subscribe({

        next: () => {

          this.doctors =
            this.doctors.filter(
              item => item.id !== doctor.id
            );


          this.applyFilters();


          this.deletingDoctorId = null;

        },


        error: (error) => {

          console.error(
            'Delete doctor failed:',
            error
          );


          this.deletingDoctorId = null;


          window.alert(
            'Unable to delete doctor. Please try again.'
          );

        }

      });

  }


  // =====================================================
  // CLEAR FILTERS
  // =====================================================

  clearFilters(): void {

    this.searchText = '';

    this.statusFilter = 'all';

    this.applyFilters();

  }

}