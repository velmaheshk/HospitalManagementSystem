using HospitalManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string AccessToken, DateTime ExpiresAt)
            GenerateAccessToken(User user);

        string GenerateRefreshToken();
    }
}
