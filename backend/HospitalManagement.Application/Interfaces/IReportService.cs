using HospitalManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.Interfaces
{
    public interface IReportService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<List<RevenueReportItem>> GetRevenueReportAsync(DateTime from, DateTime to);
        Task<List<AppointmentStatusReportItem>> GetAppointmentStatusReportAsync(DateTime from, DateTime to);
    }
}
