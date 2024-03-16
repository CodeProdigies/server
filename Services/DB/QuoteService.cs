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

    }
}
