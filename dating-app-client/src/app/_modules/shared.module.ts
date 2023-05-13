import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from "ngx-spinner";
import { FileUploadModule } from 'ng2-file-upload';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from 'ngx-timeago';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: "toast-bottom-right"
    }),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    NgxGalleryModule,
    FileUploadModule,
    NgxSpinnerModule,
    ButtonsModule.forRoot(),
    TimeagoModule.forRoot()
  ],
  exports: [
    TimeagoModule,
    ButtonsModule,
    PaginationModule,
    BsDatepickerModule,
    FileUploadModule,
    NgxSpinnerModule,
    NgxGalleryModule,
    ToastrModule,
    BsDropdownModule,
    TabsModule
  ]
})
export class SharedModule { }
