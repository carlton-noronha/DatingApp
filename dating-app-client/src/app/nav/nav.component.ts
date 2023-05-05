import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl } from "@angular/forms";
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit, OnDestroy {

  loginForm: FormGroup;

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {
    this.loginForm = new FormGroup({
      username: new FormControl(""),
      password: new FormControl("")
    });
  }

  ngOnInit(): void {}

  login() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.router.navigate(['/members']);
      }
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  ngOnDestroy(): void {
    
  }
}
