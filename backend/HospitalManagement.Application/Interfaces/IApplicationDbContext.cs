using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace HospitalManagement.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Role> Roles { get; }
        DbSet<User> Users { get; }
        DbSet<Patient> Patients { get; }
        DbSet<Department> Departments { get; }
        DbSet<Doctor> Doctors { get; }
        DbSet<DoctorAvailability> DoctorAvailabilities { get; }
        DbSet<Appointment> Appointments { get; }
        DbSet<Prescription> Prescriptions { get; }
        DbSet<PrescriptionItem> PrescriptionItems { get; }
        DbSet<Medicine> Medicines { get; }
        DbSet<Bill> Bills { get; }
        DbSet<BillItem> BillItems { get; }
        DbSet<AuditLog> AuditLogs { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
