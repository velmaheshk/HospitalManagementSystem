using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public DoctorService(IApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .Select(d => new DoctorDto(
                    d.DoctorId,
                    d.UserId,
                    d.FullName,
                    d.Specialization,
                    d.Qualification,
                    d.ExperienceYears,
                    d.ConsultationFee,
                    d.DepartmentId,
                    d.Department != null ? d.Department.DepartmentName : null))
                .ToListAsync();
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null) return null;

            return new DoctorDto(
                doctor.DoctorId,
                doctor.UserId,
                doctor.FullName,
                doctor.Specialization,
                doctor.Qualification,
                doctor.ExperienceYears,
                doctor.ConsultationFee,
                doctor.DepartmentId,
                doctor.Department?.DepartmentName);
        }

        public async Task<DoctorDto> CreateAsync(CreateDoctorRequest request)
        {
            var passwordHash = _passwordHasher.Hash(request.Password);
            var doctorRole = await _context.Roles
          .FirstOrDefaultAsync(r => r.RoleName == "Doctor");

            if (doctorRole == null)
                throw new InvalidOperationException("Doctor role not found in the database.");
            // Create the linked User account first
            var user = new User
            {
                Username = request.Username,
                PasswordHash =passwordHash, // replace with your actual hasher
                Email = request.Email,
                Phone = request.Phone,
                Role = doctorRole,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            var existingUser = await _context.Users
    .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    $"A user with email '{request.Email}' already exists.");
            }
            else
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // so user.UserId is generated

                var doctor = new Doctor
                {
                    UserId = user.UserId,
                    FullName = request.FullName,
                    Specialization = request.Specialization,
                    Qualification = request.Qualification,
                    ExperienceYears = request.ExperienceYears,
                    ConsultationFee = request.ConsultationFee,
                    DepartmentId = request.DepartmentId,

                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                var department = await _context.Departments
                    .FirstOrDefaultAsync(dep => dep.DepartmentId == doctor.DepartmentId);

                return new DoctorDto(
                    doctor.DoctorId,
                    doctor.UserId,
                    doctor.FullName,
                    doctor.Specialization,
                    doctor.Qualification,
                    doctor.ExperienceYears,
                    doctor.ConsultationFee,
                    doctor.DepartmentId,
                    department?.DepartmentName);
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorRequest request)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == id);
            if (doctor == null) return false;

            doctor.FullName = request.FullName;
            doctor.Specialization = request.Specialization;
            doctor.Qualification = request.Qualification;
            doctor.ExperienceYears = request.ExperienceYears;
            doctor.ConsultationFee = request.ConsultationFee;
            doctor.DepartmentId = request.DepartmentId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == id);
            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .Where(d => d.Specialization == specialization)
                .Select(d => new DoctorDto(
                    d.DoctorId,
                    d.UserId,
                    d.FullName,
                    d.Specialization,
                    d.Qualification,
                    d.ExperienceYears,
                    d.ConsultationFee,
                    d.DepartmentId,
                    d.Department != null ? d.Department.DepartmentName : null))
                .ToListAsync();
        }

        public async Task<DoctorDashboardDto> GetDashboardAsync()
        {
            var total = await _context.Doctors.CountAsync();

            var active = await _context.Doctors
                .Include(d => d.User)
                .CountAsync(d => d.User!.IsActive);

            var specializations = await _context.Doctors
                .Select(d => d.Specialization)
                .Distinct()
                .CountAsync();

            return new DoctorDashboardDto
            {
                TotalDoctors = total,
                ActiveDoctors = active,
                InactiveDoctors = total - active,
                TotalSpecializations = specializations
            };
        }
    }
}