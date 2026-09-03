import { Routes } from '@angular/router';

import { PatientList } from './patient-list/patient-list';
import { PatientForm } from './patient-form/patient-form';
import { PatientDetails } from './patient-details/patient-details';
import { PatientDashboardComponent } from '../features/patients/pages/patient-dashboard/patient-dashboard';

export const patientRoutes: Routes = [
{
  path:'',
  component:PatientDashboardComponent
},
  {
    path: 'list',
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