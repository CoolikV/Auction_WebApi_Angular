import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { tap } from 'rxjs/operators'

@Injectable()
export class BearerInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return null;
    }
}
