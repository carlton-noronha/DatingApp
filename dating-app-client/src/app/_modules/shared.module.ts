import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from "ngx-spinner";

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: "toast-bottom-right"
    }),
    BsDropdownModule.forRoot(),
    NgxGalleryModule,
    NgxSpinnerModule
  ],
  exports: [
    NgxSpinnerModule,
    NgxGalleryModule,
    ToastrModule,
    BsDropdownModule,
    TabsModule
  ]
})
export class SharedModule { }
