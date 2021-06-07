using System.Threading.Tasks;

namespace ExchangeAPI.Services.Interfaces
{
    public interface IBankService
    {
        Task<decimal> GetExchangeRate(string currency);
    }
}
