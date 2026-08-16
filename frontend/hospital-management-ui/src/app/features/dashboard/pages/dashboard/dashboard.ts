import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../../core/services/auth';

interface StatCard {
  title: string;
  value: number;
  icon: string;
  description: string;
}

interface Appointment {
  patient: string;
  doctor: string;
  date: string;
  time: string;
  status: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,

  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],

  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent {

  sidebarOpen = true;

  currentDate = new Date();

  stats: StatCard[] = [

    {
      title: 'Total Patients',
      value: 1250,
      icon: '👥',
      description: '+12% from last month'
    },

    {
      title: 'Total Doctors',
      value: 85,
      icon: '👨‍⚕️',
      description: '+5 new doctors'
    },

    {
      title: 'Appointments',
      value: 324,
      icon: '📅',
      description: 'Today'
    },

    {
      title: 'Revenue',
      value: 245000,
      icon: '💰',
      description: 'This month'
    }

  ];


  appointments: Appointment[] = [

    {
      patient: 'Arun Kumar',
      doctor: 'Dr. Rajesh',
      date: '16 Aug 2026',
      time: '09:30 AM',
      status: 'Confirmed'
    },

    {
      patient: 'Priya Sharma',
      doctor: 'Dr. Meena',
      date: '16 Aug 2026',
      time: '10:30 AM',
      status: 'Pending'
    },

    {
      patient: 'Suresh Kumar',
      doctor: 'Dr. John',
      date: '16 Aug 2026',
      time: '11:30 AM',
      status: 'Confirmed'
    },

    {
      patient: 'Lakshmi Devi',
      doctor: 'Dr. Anitha',
      date: '16 Aug 2026',
      time: '02:00 PM',
      status: 'Completed'
    }

  ];


  constructor(
    private router: Router,
    private authService: AuthService
  ) {}


  toggleSidebar(): void {

    this.sidebarOpen =
      !this.sidebarOpen;

  }


  logout(): void {

    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);

  }


  getUserName(): string {

    return (
      localStorage.getItem('username')
      || 'Administrator'
    );

  }

}