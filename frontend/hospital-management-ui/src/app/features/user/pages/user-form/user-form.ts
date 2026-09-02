import { CommonModule } from '@angular/common';
import { Component ,OnInit,inject} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import {
  UserApiService
} from '../../../../core/services/user-api.service';
@Component({
  selector: 'app-user-form',
  standalone:true,
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './user-form.html',
  styleUrl: './user-form.scss',
})
export class UserForm implements OnInit{
private fb = inject(FormBuilder);
  private userService = inject(UserApiService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  userForm!: FormGroup;

  userId?: number;

  isEditMode = false;
  loading = false;
  submitted = false;

  roles = [
    'Admin',
    'Doctor',
    'User'
  ];

  ngOnInit(): void {

    this.createForm();

    const id =
      this.route.snapshot.paramMap.get('id');

    if (id) {

      this.userId = Number(id);

      this.isEditMode = true;

      this.loadUser(this.userId);
    }
  }

  createForm(): void {

    this.userForm =
      this.fb.group({

        username: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(50)
          ]
        ],

        password: [
          '',
          this.isEditMode
            ? []
            : [
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

        phone: [
          '',
          [
            Validators.pattern(
              /^[0-9]{10}$/
            )
          ]
        ],

        role: [
          'User',
          Validators.required
        ]

      });
  }

  loadUser(id: number): void {

    this.loading = true;

    this.userService
      .getUserById(id)
      .subscribe({

        next: (user) => {

          this.userForm.patchValue({

            username: user.username,

            email: user.email,

            phone: user.phone,

            role: user.role

          });

          this.loading = false;
        },

        error: (error) => {

          console.error(error);

          alert(
            'Unable to load user.'
          );

          this.router.navigate([
            '/users'
          ]);
        }
      });
  }

  get f() {
    return this.userForm.controls;
  }

  saveUser(): void {

    this.submitted = true;

    if (this.userForm.invalid) {

      this.userForm.markAllAsTouched();

      return;
    }

    this.loading = true;

    const formValue =
      this.userForm.value;


    if (this.isEditMode && this.userId) {

      const request = {

        username:
          formValue.username,

        email:
          formValue.email,

        phone:
          formValue.phone,

        role:
          formValue.role,

        ...(formValue.password
          ? {
              password:
                formValue.password
            }
          : {})

      };

      this.userService
        .updateUser(
          this.userId,
          request
        )
        .subscribe({

          next: () => {

            alert(
              'User updated successfully.'
            );

            this.router.navigate([
              '/users'
            ]);
          },

          error: (error) => {

            console.error(error);

            alert(
              error?.error?.message ||
              'Unable to update user.'
            );

            this.loading = false;
          }
        });

    } else {

      this.userService
        .createUser(formValue)
        .subscribe({

          next: () => {

            alert(
              'User created successfully.'
            );

            this.router.navigate([
              '/users'
            ]);
          },

          error: (error) => {

            console.error(error);

            alert(
              error?.error?.message ||
              'Unable to create user.'
            );

            this.loading = false;
          }
        });
    }
  }
}
