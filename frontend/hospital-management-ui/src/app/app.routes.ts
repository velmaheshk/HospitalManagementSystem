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
      import('./features/auth/pages/login/login').then(
        m => m.LoginComponent
      )
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/pages/register/register').then(
        m => m.RegisterComponent
      )
  },
{
  path: 'forgot-password',
  loadComponent: () =>
    import('./features/auth/pages/forgot-password/forgot-password').then(
      m => m.ForgotPassword
    )
},{
  path: 'reset-password',
  loadComponent: () =>
    import('./features/auth/pages/reset-password/reset-password').then(
      m => m.ResetPassword
    )
},
  // =====================================================
  // ADMIN DASHBOARD
  // =====================================================
  {
    path: 'dashboard',
    canActivate: [authGuard, roleGuard(['Admin'])],
    loadComponent: () =>
      import('./features/dashboard/pages/dashboard/dashboard').then(
        m => m.DashboardComponent
      )
  },

  // =====================================================
  // DOCTOR MODULE
  // =====================================================
  {
    path: 'doctor',
    canActivate: [
      authGuard,
      // roleGuard(['Doctor'])   // enable once Doctor role claim is in place
    ],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/doctors/pages/doctor-dashboard/doctor-dashboard').then(
            m => m.DoctorDashboardComponent
          )
      },
      {
        path: 'list',
        loadComponent: () =>
          import('./features/doctors/pages/doctor-list/doctor-list').then(
            m => m.DoctorListComponent
          )
      },
      {
        path: 'add',
        loadComponent: () =>
          import('./features/doctors/pages/doctor-form/doctor-form').then(
            m => m.DoctorFormComponent
          )
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./features/doctors/pages/doctor-form/doctor-form').then(
            m => m.DoctorFormComponent
          )
      }
    ]
  },

  // =====================================================
  // Billing MODULE
  // =====================================================
  {
    path: 'billing',
      loadComponent: () =>
      import('./features/billing/bill/bill').then(
        m => m.Bill
      )
  },
{
    path: 'billitem',
      loadComponent: () =>
      import('./features/billing/billitem/billitem').then(
        m => m.Billitem
      )
  },
  // =====================================================
  // PATIENT MODULE
  // =====================================================
  {
    path: 'patient',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./patient/patient.routes').then(m => m.patientRoutes)
  },

  // =====================================================
  // APPOINTMENTS MODULE
  // =====================================================
  {
    path: 'appointments',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./core/models/appointment.routes').then(
        m => m.appointmentRoutes
      )
  },

  // =====================================================
  // USER MODULE
  // =====================================================
  {
    path: 'users',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/user/pages/user.routes').then(m => m.USER_ROUTES)
  },

  // =====================================================
  // REPORTS
  // =====================================================
  {
    path: 'report',

    canActivate: [
      authGuard,
      // roleGuard(['Admin'])
    ],

    loadComponent: () =>
      import('./features/reports/pages/report-list/report-list').then(
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