
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

interface Report {
  id: number;
  reportName: string;
  reportType: string;
  generatedBy: string;
  generatedDate: string;
  status: string;
}
@Component({
  selector: 'app-report-list',
  standalone:true,
  imports: [ CommonModule,
  RouterLink,
  FormsModule],
  templateUrl: './report-list.html',
  styleUrl: './report-list.scss',
})
export class ReportListComponent {
 searchText = '';

  reports: Report[] = [

    {
      id: 1,
      reportName: 'Daily Patient Report',
      reportType: 'Patient',
      generatedBy: 'Admin',
      generatedDate: '25 Aug 2026',
      status: 'Completed'
    },

    {
      id: 2,
      reportName: 'Doctor Performance Report',
      reportType: 'Doctor',
      generatedBy: 'Admin',
      generatedDate: '25 Aug 2026',
      status: 'Completed'
    },

    {
      id: 3,
      reportName: 'Appointment Report',
      reportType: 'Appointment',
      generatedBy: 'Admin',
      generatedDate: '24 Aug 2026',
      status: 'Completed'
    },

    {
      id: 4,
      reportName: 'Billing Summary',
      reportType: 'Billing',
      generatedBy: 'Admin',
      generatedDate: '24 Aug 2026',
      status: 'Completed'
    },

    {
      id: 5,
      reportName: 'Pharmacy Stock Report',
      reportType: 'Pharmacy',
      generatedBy: 'Admin',
      generatedDate: '23 Aug 2026',
      status: 'Completed'
    }

  ];


  get filteredReports(): Report[] {

    const search =
      this.searchText
        .trim()
        .toLowerCase();

    if (!search) {
      return this.reports;
    }

    return this.reports.filter(report =>
      report.reportName
        .toLowerCase()
        .includes(search) ||

      report.reportType
        .toLowerCase()
        .includes(search) ||

      report.generatedBy
        .toLowerCase()
        .includes(search)
    );
  }


  generateReport(): void {

    alert('Report generation screen will open.');

  }


  viewReport(report: Report): void {

    alert(
      `Opening: ${report.reportName}`
    );

  }


  downloadReport(report: Report): void {

    alert(
      `Downloading: ${report.reportName}`
    );

  }


  deleteReport(report: Report): void {

    const confirmed =
      confirm(
        `Delete "${report.reportName}"?`
      );

    if (!confirmed) {
      return;
    }

    this.reports =
      this.reports.filter(
        x => x.id !== report.id
      );

  }
}
