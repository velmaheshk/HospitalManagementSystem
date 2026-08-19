import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { PatientService } from '../../Service/patientservice';
import { Patient } from '../patient-model';

@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './patient-form.html',
  styleUrl: './patient-form.scss'
})
export class PatientForm implements OnInit {

  patientForm!: FormGroup;

  isEditMode = false;
  patientId = 0;

  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    this.createForm();

    const id = this.route.snapshot.paramMap.get('id');

    console.log('Route ID:', id);

    if (id) {

      this.isEditMode = true;
      this.patientId = Number(id);

      this.loadPatient(this.patientId);
    }
  }


  createForm(): void {

    this.patientForm = this.fb.group({

      patientId: [0],

      userId: [
        '',
        Validators.required
      ],

      fullName: [
        '',
        Validators.required
      ],

      dob: [
        '',
        Validators.required
      ],

      gender: [
        '',
        Validators.required
      ],

      address: [
        '',
        Validators.required
      ],

      bloodGroup: [
        '',
        Validators.required
      ],

      emergencyContactName: [
        '',
        Validators.required
      ],

      emergencyContactPhone: [
        '',
        Validators.required
      ]

    });
  }


  loadPatient(id: number): void {

    this.loading = true;

    console.log('Loading patient:', id);

    this.patientService.getPatientById(id).subscribe({

      next: (patient: Patient) => {

        console.log('Patient received:', patient);

        this.patientForm.patchValue({

          patientId: patient.patientId,
          userId: patient.userId,
          fullName: patient.fullName,

          // Convert date for HTML date input
          dob: patient.dob
            ? patient.dob.substring(0, 10)
            : '',

          gender: patient.gender,
          address: patient.address,
          bloodGroup: patient.bloodGroup,
          emergencyContactName: patient.emergencyContactName,
          emergencyContactPhone: patient.emergencyContactPhone

        });

        this.loading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Error loading patient:', error);

        this.errorMessage = 'Unable to load patient.';

        this.loading = false;

        this.cdr.detectChanges();
      }

    });
  }


   submitForm(): void {

    console.log('🔥 SUBMIT FORM CALLED');

    if (this.patientForm.invalid) {
      console.log('❌ FORM INVALID');
      this.patientForm.markAllAsTouched();
      return;
    }

    const patient: Patient = {
      patientId: 0,
      userId: Number(this.patientForm.value.userId),
      fullName: this.patientForm.value.fullName,
      dob: this.patientForm.value.dob,
      gender: this.patientForm.value.gender,
      address: this.patientForm.value.address,
      bloodGroup: this.patientForm.value.bloodGroup,
      emergencyContactName: this.patientForm.value.emergencyContactName,
      emergencyContactPhone: this.patientForm.value.emergencyContactPhone
    };

    console.log('📤 PATIENT BEING SENT:', patient);

    this.loading = true;

    this.patientService.addPatient(patient).subscribe({

      next: (response) => {

        console.log('✅ PATIENT SAVED:', response);

        this.loading = false;

        this.router.navigate(['/patient']);
      },

      error: (error) => {

        console.error('❌ SAVE ERROR:', error);

        this.loading = false;

        this.errorMessage =
          error?.error?.message ||
          'Failed to add patient.';

        this.cdr.detectChanges();
      }

    });
  }


  // THIS MUST BE INSIDE THE CLASS
  cancel(): void {

    console.log('Cancel clicked');

    this.router.navigate(['/patient']);

  }

}