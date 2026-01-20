import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCreds, RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountServices {

  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  baseUrl = "https://localhost:1805/api/";

  register(values: RegisterCreds) {
    return this.http.post<User>(this.baseUrl + "account/register", values).pipe(
      tap(user => {
        if (user) {
          this.setValue(user);

        }
      }

      )
    )
  }

  login(creds: LoginCreds) {
    return this.http.post<User>(this.baseUrl + "account/login", creds).pipe(
      tap(user => {
        if (user) {
          this.setValue(user);
        }
      }

      )
    )
  }

  setValue(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

}
