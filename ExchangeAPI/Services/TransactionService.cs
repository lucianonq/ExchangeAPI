using ExchangeAPI.DAOs;
using ExchangeAPI.DAOs.Interfaces;
using ExchangeAPI.DTOs;
using ExchangeAPI.Entities;
using ExchangeAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private static readonly Dictionary<Currencies, int> CurrencyMonthLimit = new()
        {
            { Currencies.USD, 200 },
            { Currencies.BRL, 300 }
        };

        private ITransactionRepository _repo;
        private IBankService _bank;

        public TransactionService(ApplicationDbContext context, IConfiguration config)
        {
            _repo = new TransactionRepository(context);
            _bank = new BankService(config);
        }

        public async Task<Transaction> Create(TransactionCreationDTO transactionUser)
        {
            var transaction = new Transaction()
            {
                UserId = transactionUser.UserId,
                Currency = (Currencies)Enum.Parse(typeof(Currencies), transactionUser.Currency),
                Date = DateTime.Today
            };

            transaction.AmountPruchased = transactionUser.AmountInPesos / await _bank.GetExchangeRate(transactionUser.Currency);

            decimal amountInMonth = await _repo.GetAmountPurchasedInThisMonth(transaction.UserId, transaction.Currency);

            if (CurrencyMonthLimit.ContainsKey(transaction.Currency) && transaction.AmountPruchased + amountInMonth < CurrencyMonthLimit[transaction.Currency])
            {
                await _repo.Save(transaction);

                return transaction;
            }
            else
            {
                throw new InvalidOperationException("The user exceded it month limit");
            }
        }
    }
}
