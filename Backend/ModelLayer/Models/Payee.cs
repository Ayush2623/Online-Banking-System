using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Models
{
    public class Payee
    {
        [Key] // Primary Key
        public int PayeeId { get; set; }

        [ForeignKey("AccountNumber")] // Foreign Key referencing Auth table
        public  long PayeeAccountNumber { get; set; }
        public Account AccountNumber { get; set; }

// [ForeignKey("FromAccount")] // Foreign Key referencing Account table
//         public long FromAccountNumber { get; set; }
//         public Account FromAccount { get; set; }

        [Required]
        public string PayeeName { get; set; }      
              
        public string Nickname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}