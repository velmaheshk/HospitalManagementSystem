using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Interface.IRepository
{
    public interface IAppointment
    {
        Task<List<Appointment>> GetAll();
        Task<Appointment> Post();
        Task<Appointment> UpdateUser(Appointment appointment);
        Task<Appointment> DeleteUser(int id);
    }
}
