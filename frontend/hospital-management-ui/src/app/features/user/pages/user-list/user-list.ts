import { CommonModule } from '@angular/common';
import { Component,OnInit,inject,ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router , RouterLink} from '@angular/router';
import { UserApiService } from '../../../../core/services/user-api.service';
 import {
  User
} from '../../../../core/models/user.model';
 
@Component({
  selector: 'app-user-list',
  standalone:true,
  imports: [CommonModule,FormsModule,RouterLink],
  templateUrl: './user-list.html',
  styleUrl: './user-list.scss',
})
export class UserList implements OnInit {
 private userService = inject(UserApiService);
  private router = inject(Router);
  
 private readonly cdr = inject(ChangeDetectorRef);
  users: User[] = [];
  filteredUsers: User[] = [];

  searchText = '';

  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadUsers();
    
  }

  loadUsers(): void {

    this.loading = true;
    this.errorMessage = '';

    this.userService.getUsers().subscribe({
      next: (response) => {
 console.log('API response:', response);
        this.users = response;
        this.filteredUsers = response;

        this.loading = false;
         this.cdr.detectChanges();
      },

      error: (error) => {

        console.error(error);

        this.errorMessage =
          'Unable to load users.';

        this.loading = false;
         this.cdr.detectChanges();
      }
    });
    this.cdr.detectChanges();
  }

  searchUsers(): void {

    const search =
      this.searchText
        .toLowerCase()
        .trim();

    if (!search) {
      this.filteredUsers = this.users;
      return;
    }

    this.filteredUsers =
      this.users.filter(user =>
        user.username
          .toLowerCase()
          .includes(search) ||

        user.email
          .toLowerCase()
          .includes(search) ||

        user.role
          .toLowerCase()
          .includes(search) ||

        (user.phone ?? '')
          .toLowerCase()
          .includes(search)
      );
  }

  editUser(id: number): void {
    this.router.navigate([
      '/users/edit',
      id
    ]);
  }

  viewUser(id: number): void {
    this.router.navigate([
      '/users/view',
      id
    ]);
  }

  deleteUser(user: User): void {

    if (!user.userId) {
      return;
    }

    const confirmed =
      confirm(
        `Are you sure you want to delete ${user.username}?`
      );

    if (!confirmed) {
      return;
    }

    this.userService
      .deleteUser(user.userId)
      .subscribe({

        next: () => {

          this.users =
            this.users.filter(
              x => x.userId !== user.userId
            );

          this.searchUsers();

          alert('User deleted successfully.');
        },

        error: (error) => {

          console.error(error);

          alert(
            'Unable to delete user.'
          );
        }
      });
  }
}
