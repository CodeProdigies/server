using Microsoft.EntityFrameworkCore;
using prod_server.Classes;
using prod_server.Entities;
using prod_server.Services.DB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace prod_server.database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
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

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Provider>()
                .HasMany(p => p.Products)
                .WithOne(p => p.Provider)
                .HasForeignKey(p => p.ProviderId);

            modelBuilder.Entity<Customer>()
                    .HasMany(a => a.Accounts)
                    .WithOne(c => c.Customer)
                    .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CustomerId);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Customer)
                .WithMany(c => c.Quotes)
                .HasForeignKey(q => q.CustomerId)
                .IsRequired(false); // Indicate that the relationship is optional


            /// Uploaded Files
            /// 
            // Configure the relationship between UploadedFile and Product
            modelBuilder.Entity<UploadedFile>()
                .HasOne<Product>() // Assuming UploadedFile has an optional relationship with Product
                .WithMany(p => p.Files) // Assuming a Product can have many UploadedFiles
                .HasForeignKey(uf => uf.ProductId) // Foreign key in UploadedFile pointing to Product
                .OnDelete(DeleteBehavior.Cascade);   // Cascade delete if Product is deleted
            
            // Configure the relationship between UploadedFile and Customer
            modelBuilder.Entity<UploadedFile>()
                .HasOne<Customer>() // Assuming UploadedFile has an optional relationship with Customer
                .WithMany(c => c.Files) // Assuming a Customer can have many UploadedFiles
                .HasForeignKey(uf => uf.CustomerId) // Foreign key in UploadedFile pointing to Customer
                .OnDelete(DeleteBehavior.Cascade);   // Cascade delete if Product is deleted

            modelBuilder.Entity<Quote>().OwnsOne(p => p.Name);
            modelBuilder.Entity<Account>().OwnsOne(p => p.Name);
            modelBuilder.Entity<Account>().OwnsOne(p => p.ShippingDetails);
            modelBuilder.Entity<Customer>().OwnsOne(p => p.ShippingDetails);
            modelBuilder.Entity<Provider>().OwnsOne(p => p.ShippingDetails);


        }
    }
}
