using System;

namespace ModelLayer.DTOs;

public class UpdateNetBankingPasswordRequest
    {
        public int AccountId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
