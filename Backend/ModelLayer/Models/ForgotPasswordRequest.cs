using System;

namespace ModelLayer.Models;

public class ForgotPasswordRequest
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
    }