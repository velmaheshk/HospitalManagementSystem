using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Interface
{
    public interface IPatient
    {
        Task<List<Patient>> GetAll();
    }
}
