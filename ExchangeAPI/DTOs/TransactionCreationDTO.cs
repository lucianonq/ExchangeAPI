namespace ExchangeAPI.DTOs
{
    public class TransactionCreationDTO
    {
        public string UserId { get; set; }
        public decimal AmountInPesos { get; set; }
        public string Currency { get; set; }
    }
}
