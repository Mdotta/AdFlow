import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  imports: [CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loading = false;
  error = '';

  constructor(private authService: Auth, private router: Router) {}

  loginWithFacebook(): void {
    this.loading = true;
    this.error = '';
    
    this.authService.loginWithFacebook()
      .then(() => {
        this.loading = false;
        this.router.navigate(['/dashboard']);
      })
      .catch((error) => {
        this.loading = false;
        this.error = error || 'Failed to login with Facebook';
        console.error('Login error:', error);
      });
  }
}

