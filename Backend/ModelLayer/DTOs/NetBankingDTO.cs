using System;

namespace ModelLayer.DTOs;

public class NetBankingDTO
    {
        public int NetBankingId { get; set; }
        public long AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        // public DateTime CreatedAt { get; set; }
    }
