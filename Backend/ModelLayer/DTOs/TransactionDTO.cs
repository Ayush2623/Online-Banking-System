namespace ModelLayer.DTOs
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public long FromAccountNumber { get; set; }
        public long ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
}