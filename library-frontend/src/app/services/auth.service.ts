import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email?: string;
  password: string;
  role?: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  role: string;
  expiration: string;
}

export interface UserSession {
  username: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'library_jwt_token';
  private readonly USER_KEY = 'library_user_info';
  private apiUrl = `${environment.apiUrl}/auth`;

  private currentUserSubject = new BehaviorSubject<UserSession | null>(this.getStoredUser());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        if (response && response.token) {
          this.setSession(response);
        }
      })
    );
  }

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    // Check if token is expired
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      if (payload.exp && Date.now() >= payload.exp * 1000) {
        this.logout();
        return false;
      }
      return true;
    } catch {
      return false;
    }
  }

  getUserRole(): string {
    const user = this.currentUserSubject.value;
    return user ? user.role : 'Guest';
  }

  isAdmin(): boolean {
    return this.getUserRole().toLowerCase() === 'admin';
  }

  getCurrentUser(): UserSession | null {
    return this.currentUserSubject.value;
  }

  private setSession(authResponse: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, authResponse.token);
    const userSession: UserSession = {
      username: authResponse.username,
      role: authResponse.role
    };
    localStorage.setItem(this.USER_KEY, JSON.stringify(userSession));
    this.currentUserSubject.next(userSession);
  }

  private getStoredUser(): UserSession | null {
    const stored = localStorage.getItem(this.USER_KEY);
    if (!stored) return null;
    try {
      return JSON.parse(stored);
    } catch {
      return null;
    }
  }
}
