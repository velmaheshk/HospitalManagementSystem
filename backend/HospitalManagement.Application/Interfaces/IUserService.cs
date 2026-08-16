using System;
using System.Collections.Generic;
using System.Text;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces
{
    /// <summary>
    /// Application-layer contract for the User Management module (admin-facing account
    /// administration — separate from AuthService, which handles login/registration).
    /// </summary>
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, UpdateUserStatusRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
