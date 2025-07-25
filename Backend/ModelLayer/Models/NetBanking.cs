using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Models
{
    public class NetBanking
    {
        [Key] // Primary Key
        public int NetBankingId { get; set; }

        [ForeignKey("Accountnumber")] // Foreign Key referencing Auth table
        public long AccountNumber { get; set; }
        public Account Accountnumber { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}