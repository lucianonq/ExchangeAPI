using ExchangeAPI.Controllers;
using ExchangeAPI.DTOs;
using ExchangeAPI.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeAPI.Tests.UnitTests
{
    [TestClass]
    public class CurrencyControllerTests: BaseTests
    {
        [TestMethod]
        public async Task GetCurrentUSDRate()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();
            var service = new BankService(config);

            var valueOfUSD = await service.GetExchangeRate(Currencies.USD);

            var controller = new CurrencyController(context, config, logger);
            var answer = await controller.Get("USD");

            Assert.AreEqual(valueOfUSD, answer.Value);
        }

        [TestMethod]
        public async Task GetCurrentBRLRate()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();
            var service = new BankService(config);

            var valueOfBRL = await service.GetExchangeRate(Currencies.BRL);

            var controller = new CurrencyController(context, config, logger);
            var answer = await controller.Get("BRL");

            Assert.AreEqual(valueOfBRL, answer.Value);
        }

        [TestMethod]
        public async Task GetCurrentRandomRate()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            var controller = new CurrencyController(context, config, logger);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                controller.Get("AUD")
            );
        }

        [TestMethod]
        public async Task WriteTransactionUSDLowerThan200()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            var controller = new CurrencyController(context, config, logger);
            var tranDTO = new TransactionCreationDTO() { UserId = "test", AmountInPesos = 10000, Currency = "USD" };
            var answer = await controller.Post(tranDTO);

            Assert.AreEqual(200, ((ObjectResult)answer).StatusCode);
        }

        [TestMethod]
        public async Task DoNotWriteTransactionUSDHigherThan200()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            var controller = new CurrencyController(context, config, logger);
            var tranDTO = new TransactionCreationDTO() { UserId = "test", AmountInPesos = 22000, Currency = "USD" };
            var answer = await controller.Post(tranDTO);

            Assert.AreEqual(403, ((ObjectResult)answer).StatusCode);
        }

        [TestMethod]
        public async Task DoNotWriteTransactionUSDHigherThan200PreviusPurchase()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            context.Transaccions.Add(new Transaction() { Currency = Currencies.USD, AmountPruchased = 199, Date = DateTime.Now, UserId = "test" });
            await context.SaveChangesAsync();

            var controller = new CurrencyController(context, config, logger);
            var tranDTO = new TransactionCreationDTO() { UserId = "test", AmountInPesos = 1000, Currency = "USD" };
            var answer = await controller.Post(tranDTO);

            Assert.AreEqual(403, ((ObjectResult)answer).StatusCode);
        }

        [TestMethod]
        public async Task WriteTransactionBRLLowerThan300()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            var controller = new CurrencyController(context, config, logger);
            var tranDTO = new TransactionCreationDTO() { UserId = "test", AmountInPesos = 5000, Currency = "BRL" };
            var answer = await controller.Post(tranDTO);

            Assert.AreEqual(200, ((ObjectResult)answer).StatusCode);
        }

        [TestMethod]
        public async Task DoNotWriteTransactionBRLHigherThan300()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            var controller = new CurrencyController(context, config, logger);
            var tranDTO = new TransactionCreationDTO() { UserId = "test", AmountInPesos = 10000, Currency = "BRL" };
            var answer = await controller.Post(tranDTO);

            Assert.AreEqual(403, ((ObjectResult)answer).StatusCode);
        }

        [TestMethod]
        public async Task DoNotWriteTransactionBRLHigherThan300PreviusPurchase()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var logger = BuildLogger();
            var config = BuildConfig();

            context.Transaccions.Add(new Transaction() { Currency = Currencies.BRL, AmountPruchased = 299, Date = DateTime.Now, UserId = "test" });
            await context.SaveChangesAsync();

            var controller = new CurrencyController(context, config, logger);
            var tranDTO = new TransactionCreationDTO() { UserId = "test", AmountInPesos = 1000, Currency = "BRL" };
            var answer = await controller.Post(tranDTO);

            Assert.AreEqual(403, ((ObjectResult)answer).StatusCode);
        }
    }
}
