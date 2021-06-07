using ExchangeAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Transaction> Transaccions { get; set; }
    }
}
