using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.IRepository;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = _context.Patients
                .FirstOrDefaultAsync(p =>p. PatientId == id);

            if (patient == null)
                return false;

            _context.Patients.Remove(await patient);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task<Patient?> UpdateAsync(
    int id,
    Patient patient)
        {
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (existingPatient == null)
                return null;

            existingPatient.FullName = patient.FullName;
            existingPatient.DOB = patient.DOB;
            existingPatient.Gender = patient.Gender;
            existingPatient.Address = patient.Address;
            existingPatient.BloodGroup = patient.BloodGroup;
            existingPatient.EmergencyContactName =
                patient.EmergencyContactName;
            existingPatient.EmergencyContactPhone =
                patient.EmergencyContactPhone;

            await _context.SaveChangesAsync();

            return existingPatient;
        }
    }
}
