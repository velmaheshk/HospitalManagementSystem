using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagement.Domain.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }

        /// <summary>Not mapped — computed. A token is usable only while unrevoked and unexpired.</summary>
        public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;
    }
}
