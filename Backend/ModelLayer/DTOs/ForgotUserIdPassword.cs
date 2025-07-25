using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs
{
    public class ForgotUserIdRequest
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email must be a Gmail address.")]

        public string Email { get; set; }
    }
}