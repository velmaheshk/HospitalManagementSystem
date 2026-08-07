using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace HospitalManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<DoctorAvailability> DoctorAvailabilities => Set<DoctorAvailability>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();
        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<BillItem> BillItems => Set<BillItem>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---- Users / Roles ----
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role).WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);

            // ---- Patients (1:1 with User) ----
            modelBuilder.Entity<Patient>().HasIndex(p => p.UserId).IsUnique();
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User).WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Patient>().Property(p => p.BloodGroup).HasMaxLength(5);

            // ---- Doctors (1:1 with User) ----
            modelBuilder.Entity<Doctor>().HasIndex(d => d.UserId).IsUnique();
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User).WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Department).WithMany(dep => dep.Doctors)
                .HasForeignKey(d => d.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Doctor>().Property(d => d.ConsultationFee).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<DoctorAvailability>()
                .HasOne(a => a.Doctor).WithMany(d => d.Availabilities)
                .HasForeignKey(a => a.DoctorId).OnDelete(DeleteBehavior.Cascade);

            // ---- Appointments ----
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor).WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>().Property(a => a.Status).HasConversion<string>();

            // ---- Prescriptions ----
            modelBuilder.Entity<Prescription>().HasIndex(p => p.AppointmentId).IsUnique();
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Appointment).WithOne(a => a.Prescription)
                .HasForeignKey<Prescription>(p => p.AppointmentId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Prescription).WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PrescriptionId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PrescriptionItem>()
                .HasOne(pi => pi.Medicine).WithMany(m => m.PrescriptionItems)
                .HasForeignKey(pi => pi.MedicineId).OnDelete(DeleteBehavior.Restrict);

            // ---- Medicines ----
            modelBuilder.Entity<Medicine>().Property(m => m.UnitPrice).HasColumnType("decimal(10,2)");

            // ---- Billing ----
            modelBuilder.Entity<Bill>().HasIndex(b => b.AppointmentId).IsUnique();
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Patient).WithMany(p => p.Bills)
                .HasForeignKey(b => b.PatientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Appointment).WithOne(a => a.Bill)
                .HasForeignKey<Bill>(b => b.AppointmentId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Bill>().Property(b => b.TotalAmount).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Bill>().Property(b => b.PaidAmount).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Bill>().Property(b => b.PaymentStatus).HasConversion<string>();

            modelBuilder.Entity<BillItem>()
                .HasOne(bi => bi.Bill).WithMany(b => b.Items)
                .HasForeignKey(bi => bi.BillId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BillItem>().Property(bi => bi.UnitPrice).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<BillItem>().Property(bi => bi.Amount).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<AuditLog>().HasKey(a => a.LogId);
            modelBuilder.Entity<DoctorAvailability>().HasKey(a => a.AvailabilityId);
            // ---- Seed lookup roles ----
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Doctor" },
                new Role { RoleId = 3, RoleName = "Patient" }
            );
        }
    }
}
