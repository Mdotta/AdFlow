import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { FacebookLoginRequest, FacebookLoginResponse, FacebookUser } from '../models/auth.models';

declare const FB: any;

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private apiUrl = 'http://localhost:5000/api';
  private currentUserSubject = new BehaviorSubject<FacebookUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      this.currentUserSubject.next(JSON.parse(userJson));
    }
  }

  loginWithFacebook(): Promise<FacebookLoginResponse> {
    return new Promise((resolve, reject) => {
      FB.login((response: any) => {
        if (response.authResponse) {
          const accessToken = response.authResponse.accessToken;
          const request: FacebookLoginRequest = { accessToken };
          
          this.http.post<FacebookLoginResponse>(`${this.apiUrl}/auth/facebook-login`, request)
            .pipe(
              tap(loginResponse => {
                localStorage.setItem('token', loginResponse.token);
                localStorage.setItem('currentUser', JSON.stringify(loginResponse.user));
                this.currentUserSubject.next(loginResponse.user);
              })
            )
            .subscribe({
              next: (loginResponse) => resolve(loginResponse),
              error: (error) => reject(error)
            });
        } else {
          reject('User cancelled login or did not fully authorize.');
        }
      }, { scope: 'public_profile,email' });
    });
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    FB.logout();
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): FacebookUser | null {
    return this.currentUserSubject.value;
  }
}

