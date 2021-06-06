using ExchangeAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.Services.Interfaces
{
    interface IBankService
    {
        Task<decimal> GetExchangeRate(string currency);
    }
}
