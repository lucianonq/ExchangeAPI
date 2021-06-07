using ExchangeAPI.DTOs;
using ExchangeAPI.Entities;
using System.Threading.Tasks;

namespace ExchangeAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> Create(TransactionCreationDTO transactionUser);
    }
}
