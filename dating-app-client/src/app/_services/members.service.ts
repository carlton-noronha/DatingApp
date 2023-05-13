import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { of } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { PaginatedResult } from '../models/pagination';
import { UserParams } from '../models/user-params';
import { AccountService } from './account.service';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  userParams: UserParams | undefined;
  user: User | undefined;
  memberCache = new Map<string, PaginatedResult<Member[]> | undefined>();
  cachedKeys: string[] = [];
  private CACHE_SIZE = 5;

  constructor(private httpClient: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user: User | null) => {
        if (user) {
          this.user = user;
          this.userParams = new UserParams(user);
        }
      },
    });
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    if(this.user) {
      this.userParams = new UserParams(this.user);
    }
  }

  getMembers(userParams: UserParams) {
    const key = Object.values(userParams).join('-');
    const response = this.memberCache.get(key);
    if (response) {
      return of(response);
    }
    const params = this.getQueryParams(userParams);
    return this.getPaginatedResults<Member[]>(
      `${this.baseUrl}users`,
      params
    ).pipe(
      tap((response) => {
        if (this.cachedKeys.length >= this.CACHE_SIZE) {
          this.memberCache.delete(this.cachedKeys.shift() || '');
        }
        this.cachedKeys.push(key);
        this.memberCache.set(key, response);
      })
    );
  }

  private getPaginatedResults<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.httpClient
      .get<T>(url, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          if (response.body) {
            paginatedResult.result = response.body;
          }
          const paginationHeader = response.headers.get('Pagination');
          if (paginationHeader) {
            paginatedResult.pagination = JSON.parse(paginationHeader);
          }
          return paginatedResult;
        })
      );
  }

  private getQueryParams(userParams: UserParams) {
    let params = new HttpParams();
    params = params.append('pageNumber', userParams.pageNumber);
    params = params.append('pageSize', userParams.pageSize);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    return params;
  }

  getMember(username: string) {
    const member = [...this.memberCache.values()].reduce<Member[]>(
      (members, paginatedResult) => {
        if (paginatedResult?.result) {
          console.log(paginatedResult)
          members = members.concat(paginatedResult.result);
        }
        return members;
      },
      []
    ).find(member => member.userName == username);
    if(member) {
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

  setMainPhoto(photoId: number) {
    return this.httpClient.put(
      `${this.baseUrl}users/set-main-photo/${photoId}`,
      {}
    );
  }

  deletePhoto(photoId: number) {
    return this.httpClient.delete(
      `${this.baseUrl}users/delete-photo/${photoId}`
    );
  }
}
