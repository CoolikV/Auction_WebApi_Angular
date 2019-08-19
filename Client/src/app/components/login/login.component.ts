import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isError: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  inputNotValid(username: string, password: string) {
    return !username || !password;
  }

  signIn(userName: string, password: string) {
    this.authService.athenticateUser(userName, password)
      .subscribe((data: any) => {
        console.log(data);
        localStorage.setItem('userToken', data.body.access_token)
        setTimeout(() => {
          this.router.navigate(['/lots'])
        }, 2000);
      },
        (err: HttpErrorResponse) => {
          this.isError = true;
        }
      )
    // (err: HttpErrorResponse) => {
    //   console.log(err.error);
  }
}