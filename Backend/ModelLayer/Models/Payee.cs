using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Models
{
    public class Payee
    {
        [Key] // Primary Key
        public int PayeeId { get; set; }

        [Required]
        public long PayeeAccountNumber { get; set; } // The payee's account number
        
        [ForeignKey("Account")] // Foreign Key referencing user's Account
        public long AccountNumber { get; set; } // User's account who added this payee
        public Account Account { get; set; }

        [Required]
        public string PayeeName { get; set; }      
              
        public string Nickname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}