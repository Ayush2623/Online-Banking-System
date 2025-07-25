using System;
using System.ComponentModel.DataAnnotations;


namespace ModelLayer.Models
{
    public class Auth
    {
        [Key] // Primary Key
        public int AuthId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
        // public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; } 

        // public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        // public DateTime UpdatedAt { get; set; }
        public Account Account { get; set; }
    }
}