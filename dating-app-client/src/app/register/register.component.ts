import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { error } from 'console';

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

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.registerFormModel).subscribe({
      next: () => {
        this.cancel();
      },
      error: error => { 
        console.log(error);
      }
    });
  }

  cancel() {
    this.closeRegistration.emit(false);
  }
}
