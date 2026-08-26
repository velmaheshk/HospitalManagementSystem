import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  User,
  CreateUserRequest,
  UpdateUserRequest
} from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {

  private http = inject(HttpClient);

  private readonly apiUrl =
    'https://localhost:44343/api/Users';

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  createUser(request: CreateUserRequest): Observable<User> {
    return this.http.post<User>(this.apiUrl, request);
  }

  updateUser(
    id: number,
    request: UpdateUserRequest
  ): Observable<User> {
    return this.http.put<User>(
      `${this.apiUrl}/${id}`,
      request
    );
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}