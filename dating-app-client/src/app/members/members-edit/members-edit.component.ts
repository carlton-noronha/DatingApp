import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-members-edit',
  templateUrl: './members-edit.component.html',
  styleUrls: ['./members-edit.component.css'],
})
export class MembersEditComponent implements OnInit {
  member: Member | undefined;
  user: User | null = null;
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    event: any
  ) {
    if (this.editForm?.dirty) {
      event.returnValue = true;
    }
  }

  constructor(
    private accountService: AccountService,
    private membersService: MembersService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user: User | null) => {
        this.user = user;
        this.loadMember();
      });
  }

  loadMember() {
    if (!this.user) {
      return;
    }
    this.membersService.getMember(this.user.userName).subscribe({
      next: (member) => (this.member = member),
    });
  }

  onUpdate() {
    this.membersService.updateMember(this.editForm?.value as Member).subscribe({
      next: () => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      },
      error: (err) => console.log(err),
    });
  }
}
