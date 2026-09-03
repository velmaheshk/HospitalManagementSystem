import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
interface QuickAction {
  title: string;
  description: string;
  icon: string;
  route: string;
  cssClass: string;
}

interface Appointment {
  id: number;
  doctorName: string;
  specialization: string;
  date: string;
  time: string;
  status: string;
  appointmentType: string;
}

interface Prescription {
  id: number;
  medicineName: string;
  dosage: string;
  frequency: string;
  duration: string;
  doctorName: string;
}

interface MedicalRecord {
  id: number;
  title: string;
  doctorName: string;
  date: string;
  type: string;
}

interface NotificationItem {
  id: number;
  title: string;
  message: string;
  time: string;
  type: string;
  unread: boolean;
}
@Component({
  selector: 'app-patient-dashboard',
  standalone:true,
  imports: [CommonModule,RouterLink],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.scss',
})
export class PatientDashboardComponent implements OnInit {
patientName = 'Sunil Joshi';
  patientId = 'PAT-2026-00125';

  today = new Date();

  // Patient information
  patientInfo = {
    age: 40,
    gender: 'Male',
    bloodGroup: 'B+',
    phone: '+91 98765 43210',
    email: 'patient@example.com',
    emergencyContact: 'Family Member',
    emergencyPhone: '+91 98765 11111'
  };

  // Dashboard statistics
  dashboardStats = {
    upcomingAppointments: 2,
    completedAppointments: 12,
    activePrescriptions: 3,
    pendingBills: 1
  };

  // Quick actions
  quickActions: QuickAction[] = [
    {
      title: 'Book Appointment',
      description: 'Schedule a new doctor appointment',
      icon: '📅',
      route: '/patient/appointments/book',
      cssClass: 'blue'
    },
    {
      title: 'My Appointments',
      description: 'View and manage appointments',
      icon: '🩺',
      route: '/patient/appointments',
      cssClass: 'green'
    },
    {
      title: 'Medical Records',
      description: 'View your medical history',
      icon: '📋',
      route: '/patient/medical-records',
      cssClass: 'purple'
    },
    {
      title: 'Prescriptions',
      description: 'View current prescriptions',
      icon: '💊',
      route: '/patient/prescriptions',
      cssClass: 'orange'
    },
    {
      title: 'Bills & Payments',
      description: 'View bills and payment history',
      icon: '💳',
      route: '/patient/billing',
      cssClass: 'red'
    },
    {
      title: 'Lab Reports',
      description: 'View and download reports',
      icon: '🧪',
      route: '/patient/lab-reports',
      cssClass: 'teal'
    }
  ];

  // Upcoming appointments
  upcomingAppointments: Appointment[] = [
    {
      id: 1,
      doctorName: 'Dr. Arun Kumar',
      specialization: 'General Medicine',
      date: '05 Sep 2026',
      time: '10:30 AM',
      status: 'Confirmed',
      appointmentType: 'Consultation'
    },
    {
      id: 2,
      doctorName: 'Dr. Priya Sharma',
      specialization: 'Cardiology',
      date: '12 Sep 2026',
      time: '03:00 PM',
      status: 'Confirmed',
      appointmentType: 'Follow-up'
    }
  ];

  // Prescriptions
  prescriptions: Prescription[] = [
    {
      id: 1,
      medicineName: 'Paracetamol 500mg',
      dosage: '1 Tablet',
      frequency: 'Twice Daily',
      duration: '5 Days',
      doctorName: 'Dr. Arun Kumar'
    },
    {
      id: 2,
      medicineName: 'Pantoprazole 40mg',
      dosage: '1 Tablet',
      frequency: 'Once Daily',
      duration: '10 Days',
      doctorName: 'Dr. Arun Kumar'
    },
    {
      id: 3,
      medicineName: 'Vitamin D3',
      dosage: '1 Capsule',
      frequency: 'Weekly',
      duration: '8 Weeks',
      doctorName: 'Dr. Priya Sharma'
    }
  ];

  // Medical records
  medicalRecords: MedicalRecord[] = [
    {
      id: 1,
      title: 'General Consultation',
      doctorName: 'Dr. Arun Kumar',
      date: '28 Aug 2026',
      type: 'Consultation'
    },
    {
      id: 2,
      title: 'Blood Test Report',
      doctorName: 'Laboratory',
      date: '25 Aug 2026',
      type: 'Lab Report'
    },
    {
      id: 3,
      title: 'Cardiology Follow-up',
      doctorName: 'Dr. Priya Sharma',
      date: '15 Aug 2026',
      type: 'Follow-up'
    }
  ];

  // Notifications
  notifications: NotificationItem[] = [
    {
      id: 1,
      title: 'Appointment Reminder',
      message: 'Your appointment with Dr. Arun Kumar is tomorrow.',
      time: '2 hours ago',
      type: 'appointment',
      unread: true
    },
    {
      id: 2,
      title: 'Lab Report Available',
      message: 'Your latest blood test report is ready.',
      time: 'Yesterday',
      type: 'report',
      unread: true
    },
    {
      id: 3,
      title: 'Payment Reminder',
      message: 'You have an outstanding hospital bill.',
      time: '2 days ago',
      type: 'billing',
      unread: false
    }
  ];

  ngOnInit(): void {
    this.loadDashboard();
  }

  constructor(private router: Router) {}

  loadDashboard(): void {
    // Replace this method with API service calls later.
    // Example:
    // this.patientService.getDashboard().subscribe(...)
  }

  bookAppointment(): void {
    this.router.navigate(['/patient/appointments/book']);
  }

  viewAppointments(): void {
    this.router.navigate(['/patient/appointments']);
  }

  viewMedicalRecords(): void {
    this.router.navigate(['/patient/medical-records']);
  }

  viewPrescriptions(): void {
    this.router.navigate(['/patient/prescriptions']);
  }

  viewBilling(): void {
    this.router.navigate(['/patient/billing']);
  }

  viewLabReports(): void {
    this.router.navigate(['/patient/lab-reports']);
  }

  viewProfile(): void {
    this.router.navigate(['/patient/profile']);
  }

  viewNotifications(): void {
    this.router.navigate(['/patient/notifications']);
  }

  viewAllRecords(): void {
    this.router.navigate(['/patient/medical-records']);
  }

  viewAllPrescriptions(): void {
    this.router.navigate(['/patient/prescriptions']);
  }

  viewAllAppointments(): void {
    this.router.navigate(['/patient/appointments']);
  }

  downloadReport(record: MedicalRecord): void {
    console.log('Downloading report:', record);
    // Implement API/download functionality here.
  }

  cancelAppointment(appointment: Appointment): void {
    const confirmed = window.confirm(
      `Are you sure you want to cancel the appointment with ${appointment.doctorName}?`
    );

    if (confirmed) {
      appointment.status = 'Cancelled';
    }
  }

  rescheduleAppointment(appointment: Appointment): void {
    this.router.navigate(
      ['/patient/appointments/reschedule', appointment.id]
    );
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return 'status-confirmed';

      case 'completed':
        return 'status-completed';

      case 'cancelled':
        return 'status-cancelled';

      case 'pending':
        return 'status-pending';

      default:
        return '';
    }
  }

  getNotificationClass(type: string): string {
    switch (type) {
      case 'appointment':
        return 'notification-appointment';

      case 'report':
        return 'notification-report';

      case 'billing':
        return 'notification-billing';

      default:
        return 'notification-default';
    }
  }

  getUnreadNotificationCount(): number {
    return this.notifications.filter(n => n.unread).length;
  }
}
