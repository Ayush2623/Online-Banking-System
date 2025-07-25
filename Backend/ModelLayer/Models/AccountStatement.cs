using System;

namespace ModelLayer.Models;

public class AccountStatement
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }