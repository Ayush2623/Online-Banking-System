using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Models
{
    public class Transaction
    {
        [Key] // Primary Key
        public int TransactionId { get; set; }

        [ForeignKey("FromAccount")] // Foreign Key referencing Account table
        public long FromAccountNumber { get; set; }
        public Account FromAccount { get; set; }

        [ForeignKey("ToAccount")] // Foreign Key referencing Account table
        public long ToAccountNumber { get; set; }
        public Account ToAccount { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
    }
}