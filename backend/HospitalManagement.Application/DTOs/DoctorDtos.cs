using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.DTOs
{

    // ---------- Auth ----------
    public record LoginRequest(string Username, string Password);
    public record RegisterRequest(string Username, string Password, string Email, string Role, string FullName);
    public record AuthResponse(string Token, string Username, string Role, int UserId);

    // ---------- Users ----------
    public record UserDto(int UserId, string Username, string Email, string? Phone, string Role, bool IsActive, DateTime CreatedAt);
    public record UpdateUserStatusRequest(bool IsActive);
    public record CreateUserRequest(string Username, string Password, string Email, string? Phone, string Role);
    public record DoctorDto(
      int DoctorId,
      int UserId,
      string FullName,
      string Specialization,
      string? Qualification,
      int? ExperienceYears,
      decimal ConsultationFee,
      int DepartmentId,
      string? DepartmentName);

     public record CreateDoctorRequest(
      string FullName,
      string Specialization,
      string? Qualification,
      int? ExperienceYears,
      decimal ConsultationFee,
      int DepartmentId,
      string Username,
      string Password,
      string Email,
      string? Phone);

    public record UpdateDoctorRequest(
       string FullName,
       string Specialization,
       string? Qualification,
       int? ExperienceYears,
       decimal ConsultationFee,
       int DepartmentId);

    // ---------- Reports ----------
    public record RevenueReportItem(string Period, decimal Revenue);
    public record AppointmentStatusReportItem(string Status, int Count);
    public record DashboardSummaryDto(int TotalPatients, int TodaysAppointments, decimal RevenueThisMonth, int LowStockCount);
}
