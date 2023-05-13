import { Component, OnInit } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { MembersService } from 'src/app/_services/members.service';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { User } from 'src/app/models/user';
import { UserParams } from 'src/app/models/user-params';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] | undefined;
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  constructor(private membersService: MembersService) {
    this.userParams = this.membersService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  onPageChanged(event: PageChangedEvent) {
    if (this.userParams && this.userParams.pageNumber != event.page) {
      this.userParams.pageNumber = event.page;
      this.membersService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }

  applyFilters() {
    if (this.userParams && this.userParams.pageNumber) {
      this.userParams.pageNumber = 1;
      this.membersService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }

  loadMembers() {
    if (this.userParams) {
      this.membersService.setUserParams(this.userParams);
      this.membersService.getMembers(this.userParams).subscribe({
        next: (response) => {
          if (response.result && response.pagination) {
            this.pagination = response.pagination;
            this.members = response.result;
          }
        },
      });
    }
  }

  resetFilters() {
    this.membersService.resetUserParams();
    this.userParams = this.membersService.getUserParams();
    this.loadMembers();
  }
}
