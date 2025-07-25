using System;

namespace ModelLayer.Models;

public class ChangePasswordRequest
    {
        public long AccountNumber { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }