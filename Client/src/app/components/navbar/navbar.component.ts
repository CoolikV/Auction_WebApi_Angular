import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AuthenticatedUser } from 'src/app/models/auth/authenticated-user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  currenUser: AuthenticatedUser = JSON.parse(localStorage.getItem('userClaims'));

  constructor(private authService: AuthService, private router: Router) {
  }

  ngOnInit() {
  }

  isLoggedIn() {
    this.currenUser = JSON.parse(localStorage.getItem('userClaims'));
    return this.authService.isUserAuthenicated();
  }

  logOut() {
    this.authService.logout();
    this.router.navigate(['/trades']);
  }

}
