import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { error } from 'console';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() closeRegistration = new EventEmitter<boolean>();
  registerFormModel: {
    username: string;
    password: string
  } = {
      username: "",
      password: ""
    };

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.registerFormModel).subscribe({
      next: () => {
        this.cancel();
      },
      error: (error: HttpErrorResponse) => { 
        this.toastr.error(error.error)
      }
    });
  }

  cancel() {
    this.closeRegistration.emit(false);
  }
}
