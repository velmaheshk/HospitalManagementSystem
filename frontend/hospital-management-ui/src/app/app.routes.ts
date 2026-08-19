import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';

export const routes: Routes = [

  // =====================================================
  // DEFAULT
  // =====================================================

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },


  // =====================================================
  // AUTH
  // =====================================================

  {
    path: 'login',

    loadComponent: () =>
      import(
        './features/auth/pages/login/login'
      ).then(
        m => m.LoginComponent
      )
  },


  {
    path: 'register',

    loadComponent: () =>
      import(
        './features/auth/pages/register/register'
      ).then(
        m => m.RegisterComponent
      )
  },


  // =====================================================
  // ADMIN DASHBOARD
  // =====================================================

  {
    path: 'dashboard',

    canActivate: [
      authGuard,
      roleGuard(['Admin'])
    ],

    loadComponent: () =>
      import(
        './features/dashboard/pages/dashboard/dashboard'
      ).then(
        m => m.DashboardComponent
      )
  },


  // =====================================================
  // DOCTOR DASHBOARD
  // =====================================================

  {
    path: 'doctor/dashboard',

    canActivate: [
      authGuard,
      roleGuard(['Doctor'])
    ],

    loadComponent: () =>
      import(
        './features/doctors/pages/doctor-dashboard/doctor-dashboard'
      ).then(
        m => m.DoctorDashboardComponent
      )
  },

    {
    path: 'patient',
    loadChildren: () =>
      import('./patient/patient.routes')
        .then(m => m.patientRoutes)
  },


  // =====================================================
  // PATIENT / NORMAL USER DASHBOARD
  // =====================================================

  {
    path: 'patient/dashboard',

    canActivate: [
      authGuard,
      roleGuard(['Patient'])
    ],

    loadComponent: () =>
      import(
        './features/patients/pages/patient-dashboard/patient-dashboard'
      ).then(
        m => m.PatientDashboardComponent
      )
  },
  

  // =====================================================
  // DOCTORS
  // =====================================================

  {
    path: 'doctor/doctor-form',

    canActivate: [
      authGuard,
        roleGuard(['Doctor'])
    ],

    loadComponent: () =>
      import(
        './features/doctors/pages/doctor-form/doctor-form'
      ).then(
        m => m.DoctorFormComponent
      )
  },
{
    path: 'doctor/doctor-list',

    canActivate: [
      authGuard,
        roleGuard(['Doctor'])
    ],

    loadComponent: () =>
      import(
        './features/doctors/pages/doctor-list/doctor-list'
      ).then(
        m => m.DoctorListComponent
      )
  },
// =====================================================
  // Reports
  // =====================================================

  {
    path: 'report',

    canActivate: [
      authGuard,
       
    ],

    loadComponent: () =>
      import(
        './features/reports/pages/report-list/report-list'
      ).then(
        m => m.ReportListComponent
      )
  },
  // =====================================================
  // FALLBACK
  // =====================================================

  {
    path: '**',
    redirectTo: 'login'
  }

];
// export const routes: Routes = [

//   {
//     path: 'patient',
//     loadChildren: () =>
//       import('./patient/patient.routes')
//         .then(m => m.patientRoutes)
//   },

//   {
//     path: '',
//     redirectTo: 'patient',
//     pathMatch: 'full'
//   }

// ];