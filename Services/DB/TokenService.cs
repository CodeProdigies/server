using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface ITokenService: IService<Token>
    {
        public Task<Token> Create(Token token);
        public Task<Token> Create(Account account);
        public Task<Token?> GetById(Guid id);
        public Task<Token?> GetByIdAndEmail(Guid id, string email);
    }
  
    public class TokenService : Service<Token>, ITokenService
    {

        public TokenService(Context database, IHttpContextAccessor contextAccessor) : base(database, contextAccessor) { }

        public async Task<Token> Create(Token token)
        {
            await _database.Tokens.AddAsync(token);
            await _database.SaveChangesAsync();
            return token;
        }

        public async Task<Token> Create(Account account)
        {
            var token = new Token(account);
            await _database.Tokens.AddAsync(token);
            await _database.SaveChangesAsync();
            return token;
        }
        
        public Task<Token?> GetById(Guid id)
        {
            return _database.Tokens.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Token?> GetByIdAndEmail(Guid id, string email)
        {
            return _database.Tokens.FirstOrDefaultAsync(x => x.Id == id && x.Email == email);
        }
        
    }
}
