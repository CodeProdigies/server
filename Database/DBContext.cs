using Microsoft.EntityFrameworkCore;
using prod_server.Entities;
using prod_server.Services.DB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace prod_server.database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Set up logging to log to the console
            //optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Quote)
                .WithMany(q => q.Products)
                .HasForeignKey(cp => cp.QuoteId);

            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductId);
        }
    }
}
