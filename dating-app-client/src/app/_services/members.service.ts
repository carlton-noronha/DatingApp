import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private httpClient: HttpClient) {}

  getMembers() {
    if (this.members.length > 0) {
      return of(this.members);
    }
    return this.httpClient.get<Member[]>(`${this.baseUrl}users`).pipe(
      tap((members) => {
        this.members = members;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find((member) => member.userName === username);
    if (member) {
      return of(member);
    }
    return this.httpClient.get<Member>(`${this.baseUrl}users/${username}`);
  }

  updateMember(member: Member) {
    return this.httpClient.put<void>(`${this.baseUrl}users`, member).pipe(
      tap(() => {
        const index = this.members.findIndex(
          (mem) => mem.userName == member.userName
        );
        if (index > -1) {
          this.members[index] = { ...this.members[index], ...member };
        }
      })
    );
  }
}
