import { Component } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-doctor-form',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './doctor-form.html',
  styleUrl: './doctor-form.scss'
})
export class DoctorFormComponent {

  doctorForm;

  constructor(
    private fb: FormBuilder
  ) {

    this.doctorForm = this.fb.group({

      fullName: [
        '',
        Validators.required
      ],

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      phone: [
        '',
        Validators.required
      ],

      specialization: [
        '',
        Validators.required
      ],

      departmentId: [
        '',
        Validators.required
      ]

    });

  }

}