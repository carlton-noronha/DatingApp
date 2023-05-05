import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './models/user';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';

  constructor(private httpClient: HttpClient, private accountService: AccountService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user') || "{}");
    if (Object.keys(user).length == 0) {
      return;
    }
    this.accountService.setCurrentUser(user);
    console.log(this.route);
    //this.router.navigateByUrl('/members');
  }
}
