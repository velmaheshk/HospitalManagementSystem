import {
  Routes
} from '@angular/router';

import {
  UserList
} from './user-list/user-list';

import {
  UserForm
} from './user-form/user-form';

import {
  UserView
} from './user-view/user-view';

export const USER_ROUTES: Routes = [

  {
    path: '',
    component: UserList
  },

  {
    path: 'add',
    component: UserForm
  },

  {
    path: 'edit/:id',
    component: UserForm
  },

  {
    path: 'view/:id',
    component: UserView
  }

];