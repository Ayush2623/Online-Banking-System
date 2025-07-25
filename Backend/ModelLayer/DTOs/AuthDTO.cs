
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.DTOs
{
    public class AuthDTO
    {
       
        public string Username { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"^(User|Admin)$", ErrorMessage = "Role must be either 'user' or 'admin'.")]
        public string Role { get; set; }


        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]

        public string MobileNumber{get; set;} 
    }
}