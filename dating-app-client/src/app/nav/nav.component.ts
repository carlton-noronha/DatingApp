import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl } from "@angular/forms";
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit, OnDestroy {

  loginForm: FormGroup;

  constructor(public accountService: AccountService) {
    this.loginForm = new FormGroup({
      username: new FormControl(""),
      password: new FormControl("")
    });
  }

  ngOnInit(): void {}

  login() {
    this.accountService.login(this.loginForm.value).subscribe({
      error: (error) => console.log(error)
    });
  }

  logout() {
    this.accountService.logout();
  }

  ngOnDestroy(): void {
    
  }
}
