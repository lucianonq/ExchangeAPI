using ExchangeAPI.DAOs.Interfaces;
using ExchangeAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DAOs
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetAmountPurchasedInThisMonth(string userId, Currencies currency)
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return await _context.Transaccions
                .Where(t => firstDayOfMonth <= t.Date && t.Date <= lastDayOfMonth && t.Currency == currency && t.UserId == userId)
                .SumAsync(c => c.AmountPruchased);
        }

        public async Task Save(Transaction tran)
        {
            _context.Add(tran);
            await _context.SaveChangesAsync();
        }
    }
}
