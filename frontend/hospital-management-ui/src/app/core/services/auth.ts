import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments';

import {
  LoginRequest,
  RegisterRequest,
  AuthResponse
} from '../models/auth.model';

import { TokenService } from './token';
 

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl =
    `${environment.apiUrl}/Auth`;

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {}

  login(
    request: LoginRequest
  ): Observable<AuthResponse> {

    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/login`,
        request
      )
      .pipe(
        tap(response => {

          this.tokenService.setTokens(
            response.accessToken,
            response.refreshToken
          );

          localStorage.setItem(
            'user',
            JSON.stringify(response)
          );
        })
      );
  }

  register(
    request: RegisterRequest
  ): Observable<AuthResponse> {

    return this.http.post<AuthResponse>(
      `${this.apiUrl}/register`,
      request
    );
  }

  logout(): Observable<any> {

    const refreshToken =
      this.tokenService.getRefreshToken();

    return this.http
      .post(
        `${this.apiUrl}/logout`,
        {
          refreshToken
        }
      )
      .pipe(
        tap(() => {
          this.tokenService.clearTokens();
          localStorage.removeItem('user');
        })
      );
  }

  isLoggedIn(): boolean {
    return !!this.tokenService.getAccessToken();
  }

  getCurrentUser(): AuthResponse | null {

    const user =
      localStorage.getItem('user');

    return user
      ? JSON.parse(user)
      : null;
  }
}
