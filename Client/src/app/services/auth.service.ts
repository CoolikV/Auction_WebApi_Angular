import { Injectable } from '@angular/core';
import { User } from '../models/user/user';
import { HttpInternalService } from './http-internal.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public routePrefix = '/api';

  private user: User;

  constructor(private httpService: HttpInternalService) {

  }

}
