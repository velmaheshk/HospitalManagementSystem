import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CreateDoctorRequest } from '../../../../core/models/create-doctor-request.model';
import { DoctorApiService } from '../../../../core/services/doctor-api.service';

@Component({
  selector: 'app-doctor-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './doctor-form.html',
  styleUrl: './doctor-form.scss'
})
export class DoctorFormComponent implements OnInit {

  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly doctorService = inject(DoctorApiService);

  doctorForm!: FormGroup;

  loading = false;
  saving = false;
  submitted = false;

  ngOnInit(): void {
    this.createForm();
  }

  private createForm(): void {
    this.doctorForm = this.fb.group({

      firstName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50)
        ]
      ],

      lastName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50)
        ]
      ],

      gender: [
        '',
        Validators.required
      ],

      dateOfBirth: [
        ''
      ],

      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[6-9]\d{9}$/)
        ]
      ],

      

username: [
  '',
  [
    Validators.required,
    Validators.email
  ]
],

password: [
  '',
  [
    Validators.required,
    Validators.minLength(6)
  ]
],
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      medicalLicenseNumber: [
        '',
        [
          Validators.required,
          Validators.maxLength(50)
        ]
      ],

      specialization: [
        '',
        [
          Validators.required
        ]
      ],

      departmentId: [
        null,
        [
          Validators.required
        ]
      ],

      qualification: [
        '',
        [
          Validators.required,
          Validators.maxLength(200)
        ]
      ],

      yearsOfExperience: [
        0,
        [
          Validators.required,
          Validators.min(0),
          Validators.max(60)
        ]
      ],

      consultationFee: [
        0,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      address: [
        '',
        [
          Validators.required,
          Validators.maxLength(500)
        ]
      ],

      city: [
        '',
        [
          Validators.required
        ]
      ],

      state: [
        '',
        [
          Validators.required
        ]
      ],

      pincode: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{6}$/)
        ]
      ],

      bio: [
        '',
        [
          Validators.maxLength(1000)
        ]
      ],

      status: [
        'Active',
        Validators.required
      ]
    });
  }

  get f() {
    return this.doctorForm.controls;
  }

  isInvalid(controlName: string): boolean {
    const control = this.doctorForm.get(controlName);

    return !!(
      control &&
      control.invalid &&
      (control.touched || this.submitted)
    );
  }

  saveDoctor(): void {

    this.submitted = true;

    if (this.doctorForm.invalid) {
      this.doctorForm.markAllAsTouched();
      return;
    }

    this.saving = true;

    const formValue  = this.doctorForm.getRawValue();
const request = {
  fullName: `${formValue.firstName} ${formValue.lastName}`.trim(),

  specialization: formValue.specialization,
  qualification: formValue.qualification,

  experienceYears: Number(formValue.yearsOfExperience),

  consultationFee: Number(formValue.consultationFee),

  departmentId: Number(formValue.departmentId),

  username: formValue.username,
  password: formValue.password,

  email: formValue.email,

  phone: formValue.phoneNumber
};

console.log('Doctor API Request:', request);

    this.doctorService.create(request).subscribe({

      next: () => {

        this.saving = false;

        alert('Doctor created successfully.');

        this.router.navigate(['/doctor/list']);
      },

      error: (error) => {

        console.error(
          'Failed to create doctor',
          error
        );

        this.saving = false;

        alert(
          error?.error?.message ??
          'Unable to create doctor. Please try again.'
        );
      }
    });
  }

  resetForm(): void {

    this.submitted = false;

    this.doctorForm.reset({
      gender: '',
      yearsOfExperience: 0,
      consultationFee: 0,
      status: 'Active'
    });
  }

  cancel(): void {
    this.router.navigate(['/doctor/list']);
  }
}