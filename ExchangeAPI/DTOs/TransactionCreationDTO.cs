using ExchangeAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DTOs
{
    public class TransactionCreationDTO
    {
        public string UserId { get; set; }
        public decimal AmountInPesos { get; set; }
        public string Currency { get; set; }
    }
}
