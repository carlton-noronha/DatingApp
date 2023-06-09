import { Injectable } from '@angular/core';
import {
  CanDeactivate
} from '@angular/router';
import { MembersEditComponent } from '../members/members-edit/members-edit.component';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard
  implements CanDeactivate<MembersEditComponent>
{
  canDeactivate(component: MembersEditComponent): boolean {
    if(component.editForm?.dirty) {
      return confirm('Are you want to continue? Unsaved changes will be lost');
    }
    return true;
  }
}
