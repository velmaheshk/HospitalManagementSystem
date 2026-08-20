import { Component,OnInit,inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  DoctorApiService
} from '../../../../core/services/doctor-api.service';

import {
  Doctor
} from '../../../../core/models/doctor.model';

@Component({
  selector: 'app-doctor-form',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './doctor-form.html',
  styleUrl: './doctor-form.scss'
})
export class DoctorFormComponent implements OnInit{

  private readonly fb =
    inject(FormBuilder);

  private readonly doctorService =
    inject(DoctorApiService);

  private readonly route =
    inject(ActivatedRoute);

  private readonly router =
    inject(Router);

  doctorId: number | null = null;

  isEditMode = false;

  loading = false;

  submitted = false;


  doctorForm = this.fb.group({

    firstName: [
      '',
      [
        Validators.required,
        Validators.minLength(2)
      ]
    ],

    lastName: [
      '',
      [
        Validators.required
      ]
    ],

    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    phoneNumber: [
      '',
      [
        Validators.required
      ]
    ],

    specialization: [
      '',
      [
        Validators.required
      ]
    ],

    qualification: [
      ''
    ],

    experienceYears: [
      0,
      [
        Validators.min(0)
      ]
    ],

    consultationFee: [
      0,
      [
        Validators.min(0)
      ]
    ],

    departmentId: [
      0
    ],

    isActive: [
      true
    ]

  });


  ngOnInit(): void {

    const id =
      this.route.snapshot.paramMap
        .get('id');

    if (id) {

      this.doctorId = +id;

      this.isEditMode = true;

      this.loadDoctor(
        this.doctorId
      );
    }

  }


  loadDoctor(id: number): void {

    this.loading = true;

    this.doctorService
      .getById(id)
      .subscribe({

        next: (doctor) => {

          this.doctorForm.patchValue({

            firstName:
              doctor.firstName,

            lastName:
              doctor.lastName,

            email:
              doctor.email,

            phoneNumber:
              doctor.phoneNumber,

            specialization:
              doctor.specialization,

            qualification:
              doctor.qualification ?? '',

            experienceYears:
              doctor.experienceYears ?? 0,

            consultationFee:
              doctor.consultationFee ?? 0,

            departmentId:
              doctor.departmentId ?? 0,

            isActive:
              doctor.isActive
          });

          this.loading = false;
        },

        error: () => {

          this.loading = false;

          alert(
            'Unable to load doctor details'
          );

        }

      });

  }


  save(): void {

    this.submitted = true;

    if (
      this.doctorForm.invalid
    ) {

      this.doctorForm.markAllAsTouched();

      return;

    }


    this.loading = true;

    const doctor =
      this.doctorForm
        .getRawValue() as Doctor;


    if (
      this.isEditMode &&
      this.doctorId
    ) {

      doctor.id =
        this.doctorId;

      this.updateDoctor(
        doctor
      );

    }
    else {

      this.createDoctor(
        doctor
      );

    }

  }


  createDoctor(
    doctor: Doctor
  ): void {

    this.doctorService
      .create(doctor)
      .subscribe({

        next: () => {

          this.router.navigate([
            '/doctor/list'
          ]);

        },

        error: (error) => {

          console.error(error);

          this.loading = false;

        }

      });

  }


  updateDoctor(
    doctor: Doctor
  ): void {

    this.doctorService
      .update(
        this.doctorId!,
        doctor
      )
      .subscribe({

        next: () => {

          this.router.navigate([
            '/doctor/list'
          ]);

        },

        error: (error) => {

          console.error(error);

          this.loading = false;

        }

      });

  }


  cancel(): void {

    this.router.navigate([
      '/doctor/list'
    ]);

  }


  get f() {

    return this.doctorForm.controls;

  }

}