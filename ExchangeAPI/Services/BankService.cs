using ExchangeAPI.Entities;
using ExchangeAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeAPI
{
    public class BankService : IBankService
    {
        private IConfiguration _config;

        public BankService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<decimal> GetExchangeRate(string currency)
        {
            var currencyEnum = (Currencies)Enum.Parse(typeof(Currencies), currency);

            switch (currencyEnum)
            {
                case Currencies.BRL:
                    return await GetRealRate();
                case Currencies.USD:
                    return await GetDollarRate();
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private async Task<decimal> GetDollarRate()
        {
            var url = _config.GetValue<string>("bankService");
            return await CallBProvService("Dolar", url);
        }

        private async Task<decimal> GetRealRate()
        {
            return await GetDollarRate() / 4;
        }

        private async Task<decimal> CallBProvService(string method, string url)
        {
            using WebClient wc = new();
            string uri = url + "/" + method;
            var json = await wc.DownloadStringTaskAsync(uri);
            dynamic deserialized = JsonConvert.DeserializeObject(json);

            return deserialized[1];
        }
    }
}
