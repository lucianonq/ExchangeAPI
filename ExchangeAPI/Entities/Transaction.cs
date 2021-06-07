using System;

namespace ExchangeAPI.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal AmountPruchased { get; set; }
        public Currencies Currency { get; set; }
        public DateTime Date { get; set; }
    }
}
