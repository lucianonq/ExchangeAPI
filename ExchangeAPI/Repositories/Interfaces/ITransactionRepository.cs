using ExchangeAPI.Entities;
using System.Threading.Tasks;

namespace ExchangeAPI.DAOs.Interfaces
{
    public interface ITransactionRepository
    {
        Task<decimal> GetAmountPurchasedInThisMonth(string userId, Currencies currency);
        Task Save(Transaction tran);
    }
}
