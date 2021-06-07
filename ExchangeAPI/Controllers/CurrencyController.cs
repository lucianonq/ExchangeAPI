using ExchangeAPI.DTOs;
using ExchangeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ExchangeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBankService _service;
        private readonly ITransactionService _tranService;

        public CurrencyController(ApplicationDbContext context, IConfiguration config, ITransactionService tranService)
        {
            _context = context;
            _service = new BankService(config);
            _tranService = tranService;
        }

        [HttpGet("rate/{currency}")]
        public async Task<ActionResult<decimal>> Get(string currency)
        {
            return await _service.GetExchangeRate(currency);
        }

        [HttpPost("purchase")]
        public async Task<ActionResult> Post(TransactionCreationDTO transactionUser)
        {
            return Ok(await _tranService.Create(transactionUser));
        }
    }
}
