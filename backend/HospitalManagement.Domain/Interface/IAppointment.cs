using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Interface
{
    public interface IAppointment
    {
        Task<List<Appointment>> GetAll();
        Task<Appointment> Post();
    }
}
