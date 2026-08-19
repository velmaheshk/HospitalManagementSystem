import { Routes } from '@angular/router';

import { PatientList } from './patient-list/patient-list';
import { PatientForm } from './patient-form/patient-form';
import { PatientDetails } from './patient-details/patient-details';

export const patientRoutes: Routes = [

  {
    path: '',
    component: PatientList
  },

  {
    path: 'add',
    component: PatientForm
  },

  {
    path: 'edit/:id',
    component: PatientForm
  },

  {
    path: 'details/:id',
    component: PatientDetails
  }

];