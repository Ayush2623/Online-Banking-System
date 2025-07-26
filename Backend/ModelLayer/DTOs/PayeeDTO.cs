namespace ModelLayer.DTOs
{
    public class PayeeDTO
    {
        // Remove PayeeId from DTO as it's not needed for adding payee
        public string PayeeName { get; set; }
        public long PayeeAccountNumber { get; set; }
        public string Nickname { get; set; }
        public long AccountNumber { get; set; } // Frontend sends this - user's account
    }
}