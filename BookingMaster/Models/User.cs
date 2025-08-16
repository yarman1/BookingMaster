﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string FirstName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(25)")]
        public string LastName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(20)")]
        public string Phone { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(100)")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column(TypeName = "datetime2(0)")] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2(0)")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int RoleId { get; set; }

        public required Role Role { get; set; }
    }
}
