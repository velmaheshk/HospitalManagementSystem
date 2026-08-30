import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
@Component({
  selector: 'app-forgot-password',
  standalone:true,
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './forgot-password.html',
  styleUrl: '../login/login.scss',
})
export class ForgotPassword {
loading = false;
  submitted = false;
  errorMessage = '';

  forgotForm;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  submit(): void {

    if (this.forgotForm.invalid) {
      this.forgotForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const email = this.forgotForm.value.email ?? '';

    this.authService.forgotPassword(email).subscribe({

      next: () => {
        this.loading = false;
        this.submitted = true;
      },

      error: error => {
        this.loading = false;
        this.errorMessage =
          error?.error?.message ??
          'Something went wrong. Please try again.';
      }

    });
  }
}
