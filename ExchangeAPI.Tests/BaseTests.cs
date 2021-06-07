
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ExchangeAPI.Tests
{
    public class BaseTests
    {
        protected ApplicationDbContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(dbName).Options;

            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        protected IConfiguration BuildConfig()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"bankService", "https://www.bancoprovincia.com.ar/Principal"}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();

            return configuration;
        }
    }
}
