import { Injectable } from '@angular/core';
import { inject } from '@angular/core';
 
@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly accessTokenKey = 'accessToken';
  private readonly refreshTokenKey = 'refreshToken';
  
  setTokens(
    accessToken: string,
    refreshToken: string
  ): void {

    localStorage.setItem(
      this.accessTokenKey,
      accessToken
    );

    localStorage.setItem(
      this.refreshTokenKey,
      refreshToken
    );
  }

  getAccessToken(): string | null {
    return  localStorage.getItem(
       this.accessTokenKey
     );
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(
      this.refreshTokenKey
    );
  }

  clearTokens(): void {
    localStorage.removeItem(
      this.accessTokenKey
    );

    localStorage.removeItem(
      this.refreshTokenKey
    );
  }
}
