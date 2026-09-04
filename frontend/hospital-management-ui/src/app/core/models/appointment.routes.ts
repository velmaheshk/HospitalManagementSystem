import { Routes } from '@angular/router';
import { AppointmentList } from '../../features/Appointment/appointment-list/appointment-list';
import { AppointmentBook } from '../../features/Appointment/appointment-book/appointment-book';

// import { AppointmentList } from './appointment-list/appointment-list';
// import { AppointmentBook } from './appointment-book/appointment-book';

export const appointmentRoutes: Routes = [

  {
    path: '',
    component: AppointmentList
  },

  {
    path: 'book',
    component: AppointmentBook
  },

  {
    path: 'edit/:id',
    component: AppointmentBook
  }

];