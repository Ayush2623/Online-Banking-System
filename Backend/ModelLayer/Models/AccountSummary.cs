using System;

namespace ModelLayer.Models;

public class AccountSummary
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> RecentTransactions { get; set; }
    }
