import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { Member } from 'src/app/models/member';
import { Photo } from 'src/app/models/photo';
import { User } from 'src/app/models/user';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;

  constructor(private accountService: AccountService, private membersService: MembersService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(
      take(1)
    ).subscribe((user: User | null) => {
      if(user) {
        this.user = user;
        this.initializeUploader();
      } else {
        console.log("Signed out!");
      }
    })
  }

  fileOverBase(event: any) {
    this.hasBaseDropZoneOver = event;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${this.baseUrl}users/add-photo`,
      authToken: `Bearer ${this.user?.token}`,
      isHTML5:  true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const photo = JSON.parse(response);
        this.member?.photos.push(photo);
      }
    }
  }

  setMainPhoto(photo: Photo) {
    this.membersService.setMainPhoto(photo.id).subscribe({
      next: () => {
        if(this.user && this.member) {
          this.user.photoUrl = photo.url
          this.accountService.setCurrentUser(this.user);
          this.member.photoUrl = photo.url;
          this.member.photos.forEach(p => {
            if(p.isMain) {
              p.isMain = false;
            }
            if(p.id == photo.id) {
              p.isMain = true;
            }
          })
        }
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  deletePhoto(photo: Photo) {
    this.membersService.deletePhoto(photo.id).subscribe({
      next: () => {
        if(this.member) {
          this.member.photos = this.member.photos.filter(p => p.id != photo.id);
        }
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
}
