using System;

namespace ModelLayer.DTOs;

public class EnableNetBankingRequest
    {
        public long AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
