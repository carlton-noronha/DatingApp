import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
  baseUrl = "https://localhost:5001/api/";
  validationErrors = [];

  constructor(private httpClient: HttpClient){ }

  ngOnInit(): void {
  }

  get404Error() {
    this.httpClient.get(
      `${this.baseUrl}buggy/not-found`
    ).subscribe({
      error: error => console.log(error)
    });
  }

  get400Error() {
    this.httpClient.get(
      `${this.baseUrl}buggy/bad-request`
    ).subscribe({
      error: error => console.log(error)
    });
  }

  get500Error() {
    this.httpClient.get(
      `${this.baseUrl}buggy/server-error`
    ).subscribe({
      error: error => console.log(error)
    });
  }

  get401Error() {
    this.httpClient.get(
      `${this.baseUrl}buggy/auth`
    ).subscribe({
      error: error => console.log(error)
    });
  }

  getValidationError() {
    this.httpClient.post(
      `${this.baseUrl}account/register`, {}
    ).subscribe({
      error: error => {
        console.log(error);
        this.validationErrors = error;
      }
    });
  }

}
