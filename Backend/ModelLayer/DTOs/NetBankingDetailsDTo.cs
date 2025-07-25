using System;

namespace ModelLayer.DTOs;

public class NetBankingDetailsDTO
    {
        public long AccountNumber { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
    }
