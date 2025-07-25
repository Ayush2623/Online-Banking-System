using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelLayer.DTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public long  AccountNumber { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email must be a Gmail address.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Aadhaar card number is required.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar card number must be exactly 12 digits.")]

        public string AadharCardNumber { get; set; }

        [Required(ErrorMessage = "Residential address is required.")]
        public string ResidentialAddress { get; set; }

        [Required(ErrorMessage = "Permanent address is required.")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "Occupation details are required.")]
        public string OccupationDetails { get; set; }

        [Required(ErrorMessage = "Account type is required.")]
        public string AccountType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
        public decimal Balance { get; set; }
        public bool EnableNetBanking { get; set; }
    }
}