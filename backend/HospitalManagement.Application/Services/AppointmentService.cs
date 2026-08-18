using AutoMapper;
using HospitalManagement.Application.DTO;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interface.IRepository;
using System.Globalization;

namespace HospitalManagement.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentResponseDto> CreateAsync(
            CreateAppointmentDto dto)
        {
            // 1. Validate patient
            var patientExists =
                await _appointmentRepository.PatientExistsAsync(dto.PatientId);

            if (!patientExists)
            {
                throw new KeyNotFoundException(
                    $"Patient with ID {dto.PatientId} was not found.");
            }

            // 2. Validate doctor
            var doctorExists =
                await _appointmentRepository.DoctorExistsAsync(dto.DoctorId);

            if (!doctorExists)
            {
                throw new KeyNotFoundException(
                    $"Doctor with ID {dto.DoctorId} was not found.");
            }

            // 3. Validate appointment date
            if (dto.AppointmentDate.Date < DateTime.UtcNow.Date)
            {
                throw new ArgumentException(
                    "Appointment date cannot be in the past.");
            }

            // 4. Parse requested time slot
            var (startTime, endTime) =
                ParseTimeSlot(dto.TimeSlot);

            // 5. Check doctor availability
            var dayOfWeek =
                dto.AppointmentDate.DayOfWeek.ToString();

            var availabilities =
                await _appointmentRepository
                    .GetDoctorAvailabilityAsync(
                        dto.DoctorId,
                        dayOfWeek);

            if (!availabilities.Any())
            {
                throw new InvalidOperationException(
                    $"Doctor is not available on {dayOfWeek}.");
            }

            // 6. Check slot inside availability
            var isWithinAvailability =
                availabilities.Any(a =>
                    startTime >= a.StartTime &&
                    endTime <= a.EndTime);

            if (!isWithinAvailability)
            {
                throw new InvalidOperationException(
                    "The selected time slot is outside the doctor's availability.");
            }

            // 7. Check start < end
            if (startTime >= endTime)
            {
                throw new ArgumentException(
                    "Appointment start time must be before end time.");
            }

            // 8. Check duplicate booking
            var alreadyBooked =
                await _appointmentRepository
                    .IsSlotAlreadyBookedAsync(
                        dto.DoctorId,
                        dto.AppointmentDate,
                        dto.TimeSlot);

            if (alreadyBooked)
            {
                throw new InvalidOperationException(
                    "The selected time slot is already booked.");
            }

            // 9. Map DTO -> Entity
            var appointment =
                _mapper.Map<Appointment>(dto);

            appointment.Status =
                AppointmentStatus.Scheduled;

            appointment.CreatedAt =
                DateTime.UtcNow;

            // 10. Save
            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            // 11. Reload with Patient and Doctor
            var createdAppointment =
                await _appointmentRepository
                    .GetByIdAsync(appointment.AppointmentId);

            if (createdAppointment == null)
            {
                throw new InvalidOperationException(
                    "Appointment was created but could not be retrieved.");
            }

            return _mapper.Map<AppointmentResponseDto>(
                createdAppointment);
        }

        public async Task<AppointmentResponseDto?> GetByIdAsync(
            int appointmentId)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(
                    appointmentId);

            if (appointment == null)
            {
                return null;
            }

            return _mapper.Map<AppointmentResponseDto>(
                appointment);
        }

        public async Task<List<AppointmentResponseDto>> GetAllAsync()
        {
            var appointments =
                await _appointmentRepository.GetAllAsync();

            return _mapper.Map<List<AppointmentResponseDto>>(
                appointments);
        }

        public async Task<List<AppointmentResponseDto>> GetByPatientIdAsync(
            int patientId)
        {
            var appointments =
                await _appointmentRepository
                    .GetByPatientIdAsync(patientId);

            return _mapper.Map<List<AppointmentResponseDto>>(
                appointments);
        }

        public async Task<List<AppointmentResponseDto>> GetByDoctorIdAsync(
            int doctorId)
        {
            var appointments =
                await _appointmentRepository
                    .GetByDoctorIdAsync(doctorId);

            return _mapper.Map<List<AppointmentResponseDto>>(
                appointments);
        }

        public async Task<List<AppointmentResponseDto>>
            GetByDoctorAndDateAsync(
                int doctorId,
                DateTime appointmentDate)
        {
            var appointments =
                await _appointmentRepository
                    .GetByDoctorAndDateAsync(
                        doctorId,
                        appointmentDate);

            return _mapper.Map<List<AppointmentResponseDto>>(
                appointments);
        }

        public async Task<AppointmentResponseDto?> UpdateAsync(
            int appointmentId,
            UpdateAppointmentDto dto)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                return null;
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "Cancelled appointment cannot be updated.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Completed appointment cannot be updated.");
            }

            if (dto.AppointmentDate.Date < DateTime.UtcNow.Date)
            {
                throw new ArgumentException(
                    "Appointment date cannot be in the past.");
            }

            var (startTime, endTime) =
                ParseTimeSlot(dto.TimeSlot);

            if (startTime >= endTime)
            {
                throw new ArgumentException(
                    "Appointment start time must be before end time.");
            }

            // Check doctor availability for new date
            var dayOfWeek =
                dto.AppointmentDate.DayOfWeek.ToString();

            var availabilities =
                await _appointmentRepository
                    .GetDoctorAvailabilityAsync(
                        appointment.DoctorId,
                        dayOfWeek);

            if (!availabilities.Any())
            {
                throw new InvalidOperationException(
                    $"Doctor is not available on {dayOfWeek}.");
            }

            var isWithinAvailability =
                availabilities.Any(a =>
                    startTime >= a.StartTime &&
                    endTime <= a.EndTime);

            if (!isWithinAvailability)
            {
                throw new InvalidOperationException(
                    "The selected time slot is outside the doctor's availability.");
            }

            // Check duplicate slot excluding current appointment
            var alreadyBooked =
                await _appointmentRepository
                    .IsSlotAlreadyBookedAsync(
                        appointment.DoctorId,
                        dto.AppointmentDate,
                        dto.TimeSlot,
                        appointmentId);

            if (alreadyBooked)
            {
                throw new InvalidOperationException(
                    "The selected time slot is already booked.");
            }

            // Map only allowed fields
            appointment.AppointmentDate =
                dto.AppointmentDate;

            appointment.TimeSlot =
                dto.TimeSlot;

            appointment.Reason =
                dto.Reason;

            await _appointmentRepository
                .UpdateAsync(appointment);

            await _appointmentRepository
                .SaveChangesAsync();

            var updatedAppointment =
                await _appointmentRepository
                    .GetByIdAsync(appointmentId);

            return updatedAppointment == null
                ? null
                : _mapper.Map<AppointmentResponseDto>(
                    updatedAppointment);
        }

        public async Task<bool> CancelAsync(
            int appointmentId)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                return false;
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Completed appointment cannot be cancelled.");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "Appointment is already cancelled.");
            }

            appointment.Status =
                AppointmentStatus.Cancelled;

            await _appointmentRepository
                .UpdateAsync(appointment);

            await _appointmentRepository
                .SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteAsync(
            int appointmentId)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                return false;
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "Cancelled appointment cannot be completed.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Appointment is already completed.");
            }

            appointment.Status =
                AppointmentStatus.Completed;

            await _appointmentRepository
                .UpdateAsync(appointment);

            await _appointmentRepository
                .SaveChangesAsync();

            return true;
        }

        private static (TimeSpan Start, TimeSpan End)
            ParseTimeSlot(string timeSlot)
        {
            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                throw new ArgumentException(
                    "Time slot is required.");
            }

            var parts =
                timeSlot.Split(
                    " - ",
                    StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                throw new ArgumentException(
                    "Invalid time slot format. " +
                    "Expected format: 10:30 AM - 10:45 AM");
            }

            if (!DateTime.TryParse(
                    parts[0],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var start))
            {
                throw new ArgumentException(
                    "Invalid start time in time slot.");
            }

            if (!DateTime.TryParse(
                    parts[1],
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var end))
            {
                throw new ArgumentException(
                    "Invalid end time in time slot.");
            }

            return (start.TimeOfDay, end.TimeOfDay);
        }
    }
}