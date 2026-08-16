using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Application.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
