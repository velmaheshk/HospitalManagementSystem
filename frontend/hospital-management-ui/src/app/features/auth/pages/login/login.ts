import { Component } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {

  loading = false;
  errorMessage = '';

  loginForm;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    // Initialize form AFTER FormBuilder is injected
    this.loginForm = this.fb.group({

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      password: [
        '',
        Validators.required
      ]

    });

  }

  login(): void {

    if (this.loginForm.invalid) {

      this.loginForm.markAllAsTouched();

      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const loginRequest = {
      email: this.loginForm.value.email ?? '',
      password: this.loginForm.value.password ?? ''
    };

    this.authService
      .login(loginRequest)
      .subscribe({

        next: response => {

          this.loading = false;
          // Save JWT + refresh token + role
       //   this.authService.saveTokens(response);
          console.log('Login successful:', response);

          if (response.role === 'Admin') {

            this.router.navigate([
              '/dashboard'
            ]);

          } else if (response.role === 'Doctor') {

            this.router.navigate([
              '/doctor/dashboard'
            ]);

          } else if (response.role === 'Patient') {

            this.router.navigate([
              '/patient/dashboard'
            ]);

          }
          else {

            // this.router.navigate([
            //   '/patient/dashboard'
            // ]);
            this.errorMessage =
              'Unknown user role.';
          }

        },

        error: error => {

          this.loading = false;

          console.error(
            'Login error:',
            error
          );

          this.errorMessage =
            error?.error?.message ??
            'Invalid username or password.';

        }

      });
  }
}