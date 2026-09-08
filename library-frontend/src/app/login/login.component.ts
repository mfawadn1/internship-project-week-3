import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  registerForm!: FormGroup;
  isRegisterMode: boolean = false;
  isLoading: boolean = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  returnUrl: string = '/';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // If already logged in, redirect home
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/']);
    }

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(4)]]
    });

    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['User', [Validators.required]]
    });
  }

  toggleMode(): void {
    this.isRegisterMode = !this.isRegisterMode;
    this.errorMessage = null;
    this.successMessage = null;
  }

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Invalid credentials or server connection error.';
      }
    });
  }

  onRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.authService.register(this.registerForm.value).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.successMessage = 'Registration successful! You can now log in with your credentials.';
        this.isRegisterMode = false;
        this.loginForm.patchValue({ username: this.registerForm.value.username });
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Registration failed. Please try a different username.';
      }
    });
  }

  // Quick fill for testing
  fillAdmin(): void {
    this.loginForm.patchValue({
      username: 'admin',
      password: 'Admin123!'
    });
  }
}
