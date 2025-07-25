using System;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class PendingAccount
    {
        [Key] // Primary Key
        public int RequestId { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key referencing Auth table

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
        [Required]
        public decimal Balance { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
         public bool IsNetBankingEnabled { get; set; } // Flag to enable NetBanking
        
    }
}