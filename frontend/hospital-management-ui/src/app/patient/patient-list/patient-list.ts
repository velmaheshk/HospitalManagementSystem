import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { PatientService } from '../../Service/patientservice';
import { Patient } from '../patient-model';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-list.html',
  styleUrl: './patient-list.scss'
})
export class PatientList implements OnInit {

  patients: Patient[] = [];

  loading = false;
  errorMessage = '';

  constructor(
    private patientService: PatientService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadPatients();
  }

  loadPatients(): void {

    this.loading = true;

    this.patientService.getPatients().subscribe({

      next: (data: Patient[]) => {

        console.log('API DATA:', data);

        this.patients = data;

        console.log('PATIENTS:', this.patients);
        console.log('COUNT:', this.patients.length);

        this.loading = false;

        // IMPORTANT
        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Error loading patients:', error);

        this.loading = false;
        this.errorMessage = 'Unable to load patients.';

        this.cdr.detectChanges();
      }

    });
  }

  addPatient(): void {
  this.router.navigate(['/patient/add']);
}

viewPatient(id: number): void {
  this.router.navigate(['/patient/details', id]);
}

editPatient(id: number): void {
  this.router.navigate(['/patient/edit', id]);
}

deletePatient(id: number): void {

  if (!confirm('Are you sure you want to delete this patient?')) {
    return;
  }

  this.patientService.deletePatient(id).subscribe({
    next: () => {

      console.log('Patient deleted');

      this.patients = this.patients.filter(
        patient => patient.patientId !== id
      );

      this.cdr.detectChanges();
    },

    error: (error) => {
      console.error('Delete failed:', error);
    }
  });

}
}