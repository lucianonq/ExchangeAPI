using ExchangeAPI.DTOs;
using ExchangeAPI.Entities;
using ExchangeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public CurrencyController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _service = new BankService(config);
        }

        [HttpGet("rate/{currency}")]
        public async Task<ActionResult<decimal>> ExchangeRate(string currency)
        {
            try
            {
                return await _service.GetExchangeRate(currency);
            }
            catch (InvalidEnumArgumentException)
            {
                return NotFound(new { message = "Currency not implemented in our exchange" });
            }
            catch (WebException)
            {
                return StatusCode(500, new { message = "One of our providers is not working" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "There was a server error" });
            }
        }

        [HttpPost("purchase")]
        public async Task<ActionResult> Post(TransactionCreationDTO transactionUser)
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var transaction = new Transaction
            {
                UserId = transactionUser.UserId,
                AmountPruchased = transactionUser.AmountInPesos / await _service.GetExchangeRate(transactionUser.Currency),
                Currency = (Currencies)Enum.Parse(typeof(Currencies), transactionUser.Currency),
                Date = DateTime.Now
            };

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
