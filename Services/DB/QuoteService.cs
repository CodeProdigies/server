using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IQuoteService
    {
        public Task<Quote> Create(Quote quote);
        public Task<List<Quote>> GetQuotes();
        public Task<Quote?> Get(Guid id);   
    }

    public class QuoteService : IQuoteService
    {
        private readonly Context _database;

        public QuoteService(Context database)
        {
            _database = database;
        }

        async public Task<Quote> Create(Quote quote)
        {
            await _database.Quotes.AddAsync(quote);
            await _database.SaveChangesAsync();
            return quote;
        }

        public Task<List<Quote>> GetQuotes()
        {
            return _database.Quotes.OrderByDescending(q => q.CreatedAt).ToListAsync();
        }

        private IQueryable<Quote> GetQuotesWithProducts()
        {
            return _database.Quotes
                .Select(q => new Quote
                {
                    Id = q.Id,
                    Name = q.Name,
                    Description = q.Description,
                    EmailAddress = q.EmailAddress,
                    ContactName = q.ContactName,
                    PhoneNumber = q.PhoneNumber,
                    TypeOfBusiness = q.TypeOfBusiness,
                    CreatedAt = q.CreatedAt,
                    // Include other Quote properties here...
                    Products = q.Products.Select(cp => new CartProduct
                    {
                        Id = cp.Id,
                        ProductId = cp.ProductId,
                        Quantity = cp.Quantity,
                        Product = cp.Product,
                        QuoteId = cp.QuoteId
                    }).ToList()
                });
        }

        public Task<Quote?> Get(Guid id)
        {
            return GetQuotesWithProducts().FirstOrDefaultAsync(q => q.Id == id);
        }
            
        


    }
}
