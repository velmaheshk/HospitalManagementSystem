using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interface
{
    public interface IBillRepository
    {
        Task<List<Bill>> GetAll();

        Task<Bill?> GetById(int billId);

        Task<Bill> Add(Bill bill);

        Task<Bill> Update(Bill bill);

        Task<bool> Delete(int billId);
    }
}