using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.Services
{
    public class BillService : IBillRepository
    {
        public Task<Bill> Add(Bill bill)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int billId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Bill>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Bill?> GetById(int billId)
        {
            throw new NotImplementedException();
        }

        public Task<Bill> Update(Bill bill)
        {
            throw new NotImplementedException();
        }
    }
}
