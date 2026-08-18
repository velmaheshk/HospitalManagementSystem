using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interface.IRepository;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByDoctorAndDateAsync(
            int doctorId,
            DateTime appointmentDate)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate.Date == appointmentDate.Date)
                .OrderBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _context.Patients
                .AnyAsync(p => p.PatientId == patientId);
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _context.Doctors
                .AnyAsync(d => d.DoctorId == doctorId);
        }

        public async Task<List<DoctorAvailability>> GetDoctorAvailabilityAsync(
            int doctorId,
            string dayOfWeek)
        {
            return await _context.DoctorAvailabilities
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.DayOfWeek.ToLower() == dayOfWeek.ToLower())
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<bool> IsSlotAlreadyBookedAsync(
            int doctorId,
            DateTime appointmentDate,
            string timeSlot,
            int? excludeAppointmentId = null)
        {
            var query = _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate.Date == appointmentDate.Date &&
                    a.TimeSlot == timeSlot &&
                    a.Status != AppointmentStatus.Cancelled);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentId != excludeAppointmentId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}