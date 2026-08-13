using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.IRepository
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient> CreateAsync(Patient patient);
        Task<Patient?> UpdateAsync(int id, Patient patient);
        Task<bool> DeleteAsync(int id);

    }
}
