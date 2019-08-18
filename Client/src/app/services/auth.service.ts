import { Injectable } from '@angular/core';
import { User } from '../models/user/user';
import { HttpInternalService } from './http-internal.service';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { UserRegister } from '../models/auth/user-register';
import { catchError, map } from 'rxjs/operators';

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

  isUserAuthenicated() {
    return localStorage.getItem('userToken');
  }

  logout() {
    localStorage.removeItem('userToken');
  }
} 
