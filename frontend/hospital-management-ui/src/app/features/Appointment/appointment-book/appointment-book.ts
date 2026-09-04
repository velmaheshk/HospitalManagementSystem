import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { CreateAppointmentDto, AppointmentService } from '../../../Service/appointment';
import { DoctorApiService } from '../../../core/services/doctor-api.service';
import { Doctor } from '../../../core/models/doctor.model';

@Component({
  selector: 'app-appointment-book',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './appointment-book.html',
  styleUrl: './appointment-book.scss'
})
export class AppointmentBook implements OnInit {

  appointmentForm!: FormGroup;

  doctors: Doctor[] = [];
  loadingDoctors = false;

  loading = false;

  constructor(
    private fb: FormBuilder,
    private AppointmentService: AppointmentService,
    private doctorService: DoctorApiService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.appointmentForm = this.fb.group({

      patientId: [
        '',
        Validators.required
      ],

      doctorId: [
        '',
        Validators.required
      ],

      appointmentDate: [
        '',
        Validators.required
      ],

      timeSlot: [
        '',
        Validators.required
      ],

      reason: [
        ''
      ]

    });

    this.loadDoctors();
  }

  private loadDoctors(): void {

    this.loadingDoctors = true;

    this.doctorService.getAll().subscribe({

      next: (data) => {

        this.doctors = data;

        this.loadingDoctors = false;
      },

      error: (error) => {

        console.error(
          'Failed to load doctors',
          error
        );

        this.loadingDoctors = false;
      }

    });
  }

  submit(): void {

    if (this.appointmentForm.invalid) {

      this.appointmentForm.markAllAsTouched();

      return;
    }


    const dto: CreateAppointmentDto = {

      patientId:
        Number(
          this.appointmentForm.value.patientId
        ),

      doctorId:
        Number(
          this.appointmentForm.value.doctorId
        ),

      appointmentDate:
        this.appointmentForm.value.appointmentDate,

      timeSlot:
        this.appointmentForm.value.timeSlot,

      reason:
        this.appointmentForm.value.reason || null

    };


    this.loading = true;


    this.AppointmentService
      .createAppointment(dto)
      .subscribe({

        next: (response) => {

          console.log(
            'Appointment created:',
            response
          );

          this.loading = false;

          alert(
            'Appointment booked successfully.'
          );

          this.router.navigate([
            '/appointments'
          ]);

        },

        error: (error) => {

          console.error(
            'Create appointment error:',
            error
          );

          this.loading = false;

          alert(
            error?.error?.message ||
            'Failed to book appointment.'
          );

        }

      });

  }


  cancel(): void {

    this.router.navigate([
      '/appointments'
    ]);

  }

}