using ExchangeAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected ILogger<CurrencyController> BuildLogger()
        {
            var logger = Mock.Of<ILogger<CurrencyController>>();

            return logger;
        }
    }
}
