import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import {
  CommonModule
} from '@angular/common';

import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';

import {
  UserApiService
} from '../../../../core/services/user-api.service';

import {
  User
} from '../../../../core/models/user.model';

@Component({
  selector: 'app-user-view',
  imports: [CommonModule, RouterLink],
  templateUrl: './user-view.html',
  styleUrl: './user-view.scss',
})
export class UserView implements OnInit {
  private route = inject(ActivatedRoute);

  private userService =
    inject(UserApiService);

  private cdr = inject(ChangeDetectorRef);   // added

  user?: User;

  loading = true;

  ngOnInit(): void {

    const id =
      Number(
        this.route.snapshot.paramMap.get('id')
      );

    this.userService
      .getUserById(id)
      .subscribe({

        next: (response) => {

          this.user = response;

          this.loading = false;

          this.cdr.detectChanges();   // added
        },

        error: (error) => {

          console.error(error);

          this.loading = false;

          this.cdr.detectChanges();   // added
        }

      });
  }
}