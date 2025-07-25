using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Models
{
    public class Account
    {
        [Key] // Primary Key
        public int AccountId { get; set; }

        [ForeignKey("Auth")] // Foreign Key referencing Auth table
        public int UserId { get; set; }
        public Auth Auth { get; set; }

        
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AccountNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string AadharCardNumber { get; set; }

        public string ResidentialAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string OccupationDetails { get; set; }

        [Required]
        public string AccountType { get; set; }

        public decimal Balance { get; set; }
         public DateTime CreatedAt { get; set; }
        // public DateTime UpdatedAt { get; set; }
        public string Password { get; set; }

        // public string ResetToken { get; set; }
        // public DateTime? ResetTokenExpiry { get; set; }

        public bool IsNetBankingEnabled { get; set; } // New property
    //     public string Otp { get; set; }  // Added property for OTP
    // public DateTime? OtpExpiry { get; set; }
    }
    
}