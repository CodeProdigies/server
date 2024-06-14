using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IQuoteService: IService<Quote>
    {
        public Task<Quote> Create(Quote quote);
        public Task<List<Quote>> GetQuotes();
        public Task<Quote?> Get(Guid id);
        public Task Delete(Guid id);
        public Task<List<Quote>> GetWithProducts();
    }

    public class QuoteService : Service<Quote>, IQuoteService
    {
        public QuoteService(Context database, IHttpContextAccessor contextAccessor) : base(database, contextAccessor) { }

        async public Task<Quote> Create(Quote quote)
        {
            await _database.Quotes.AddAsync(quote);
            await _database.SaveChangesAsync();
            return quote;
        }

        public Task<List<Quote>> GetQuotes()
        {
            return _database.Quotes.OrderByDescending(q => q.CreatedAt).Include(x => x.Name).ToListAsync();
        }

        public Task<List<Quote>> GetWithProducts()
        {
            return GetQuotesWithProducts().ToListAsync();
        }


        private IQueryable<Quote> GetQuotesWithProducts()
        {
            return _database.Quotes
                .Include(q => q.Name)
                .Include(q => q.Products)
                .ThenInclude(cp => cp.Product);
        }

        public Task<Quote?> Get(Guid id)
        {
            return GetQuotesWithProducts().FirstOrDefaultAsync(q => q.Id == id);
        }


        async public Task Delete(Guid id)
        {
            var quote = _database.Quotes.FirstOrDefault(quote => quote.Id == id);
            if (quote == null) throw new Exception("Quote not found.");

            _database.Remove(quote);
            await _database.SaveChangesAsync();
        }

    }
}
