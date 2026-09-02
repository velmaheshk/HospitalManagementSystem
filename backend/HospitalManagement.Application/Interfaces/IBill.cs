using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.Interfaces
{
    public interface IBill
    {
        Task<List<Bill>> GetAll();

        Task<Bill?> GetById(int billId);

        Task<Bill> Add(Bill bill);

        Task<Bill> Update(Bill bill);

        Task<bool> Delete(int billId);
    }
}
