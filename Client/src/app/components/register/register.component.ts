import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserRegister } from 'src/app/models/auth/user-register';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  isSuccess: boolean = true;
  errors: any[];
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  isNotValid(username: string, password: string, confPass: string, email: string, name: string, surname: string, dateOfBirth: Date): boolean {
    return !username || !password || !confPass || !email || !name || !surname || !dateOfBirth;
  }

  register(username: string, password: string, confPass: string, email: string, name: string, surname: string, dateOfBirth: Date) {
    let newUser: UserRegister = {
      name: name,
      surname: surname,
      email: email,
      password: password,
      confirmPassword: confPass,
      userName: username,
      birthDate: dateOfBirth
    };
    console.log(newUser);
    this.authService.registerNewUser(newUser).subscribe(
      (resp) => {
        this.router.navigate(['login']);
      },
      (err: HttpErrorResponse) => {
        console.log(err.error.ModelState);
        this.isSuccess = false;
      }
    )
  }
}
