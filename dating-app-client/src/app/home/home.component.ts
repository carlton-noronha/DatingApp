import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.httpClient.get("https://localhost:5001/api/users").subscribe({
      next: (users) => {
        this.users = users;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log("Request completed!");
      }
    });
  }

  onCloseRegistration(inRegisterMode: boolean) {
    this.registerMode = inRegisterMode
  }
}