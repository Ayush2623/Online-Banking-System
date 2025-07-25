using System;

namespace ModelLayer.Models;

public class SetNewPasswordRequest
{
    public int  UserId { get; set; }
    public string NewPassword { get; set; }
    public string ResetToken { get; set; }
}