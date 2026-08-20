import { Component, OnInit, inject } from '@angular/core';
// import { RouterLink } from '@angular/router';

import {
  DoctorApiService
} from '../../../../core/services/doctor-api.service';

    import { doctordashboard } from '../../../../core/models/doctordashboard.model';
@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [ ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.scss',
})
export class DoctorDashboardComponent implements OnInit {
  private readonly doctorService =
    inject(DoctorApiService);

  dashboard: doctordashboard | null = null;

  loading = false;

  errorMessage = '';

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {

    this.loading = true;

    this.doctorService
      .getDashboard()
      .subscribe({

        next: (response) => {

          this.dashboard = response;

          this.loading = false;
        },

        error: () => {

          this.errorMessage =
            'Failed to load doctor dashboard';

          this.loading = false;
        }

      });
  }
}
