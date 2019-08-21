import { Injectable } from '@angular/core';
import { User } from '../models/user/user';
import { HttpInternalService } from './http-internal.service';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { UserRegister } from '../models/auth/user-register';
import { catchError, map } from 'rxjs/operators';
import { AuthenticatedUser } from '../models/auth/authenticated-user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public routePrefix = '/api';

  private user: User;

  constructor(private httpService: HttpInternalService, private http: HttpClient) {
  }

  athenticateUser(userName: string, password: string) {
    let data = "username=" + userName + "&password=" + password + "&grant_type=password";
    return this.httpService.postFullRequest<any>("/Token", data);
  }

  registerNewUser(newUser: UserRegister) {
    return this.httpService.postFullRequest(this.routePrefix + "/register", newUser);
  }

  isUserAuthenicated(): boolean {
    return localStorage.getItem('userToken') != null;
  }

  getToken() {
    return localStorage.getItem('userToken');
  }

  getUserClaims() {
    return this.httpService.getRequest<AuthenticatedUser>(this.routePrefix + '/accounts/claims')
  }

  recieveClaims(): AuthenticatedUser {
    let claims = localStorage.getItem('userClaims')
    return JSON.parse(claims);
  }

  logout() {
    localStorage.removeItem('userToken');
    localStorage.removeItem('userClaims');
  }
} 
