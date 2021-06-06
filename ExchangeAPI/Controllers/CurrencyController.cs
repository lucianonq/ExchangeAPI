using ExchangeAPI.DTOs;
using ExchangeAPI.Entities;
using ExchangeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBankService _service;
        private readonly ILogger _logger;

        public CurrencyController(ApplicationDbContext context, IConfiguration config, ILogger<CurrencyController> logger)
        {
            _context = context;
            _service = new BankService(config);
            _logger = logger;
        }

        [HttpGet("rate/{currency}")]
        public async Task<ActionResult<decimal>> Get(string currency)
        {
            var currencyEnum = (Currencies)Enum.Parse(typeof(Currencies), currency);

            return await _service.GetExchangeRate(currencyEnum);
        }

        [HttpPost("purchase")]
        public async Task<ActionResult> Post(TransactionCreationDTO transactionUser)
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var transaction = new Transaction()
            {
                UserId = transactionUser.UserId,
                Currency = (Currencies)Enum.Parse(typeof(Currencies), transactionUser.Currency),
                Date = DateTime.Today
            };

            transaction.AmountPruchased = transactionUser.AmountInPesos / await _service.GetExchangeRate(transaction.Currency);

            var amountInMonth = await _context.Transaccions.Where(t => firstDayOfMonth <= t.Date && t.Date <= lastDayOfMonth && t.Currency == transaction.Currency).SumAsync(c => c.AmountPruchased);

            if ((transaction.Currency == Currencies.USD && amountInMonth + transaction.AmountPruchased <= 200) || (transaction.Currency == Currencies.BRL && amountInMonth + transaction.AmountPruchased <= 300))
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();

                return Ok(transaction);
            }
            else
            {
                return StatusCode(403, new { message = "You exceded your purchase limit in this currency" });
            }
        }
    }
}
