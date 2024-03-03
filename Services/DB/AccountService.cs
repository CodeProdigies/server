using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;


namespace prod_server.Services.DB
{
    public interface IAccountService
    {
        Task<Account?> Create(RegisterModel registerModel);
        Task<Account?> GetAccountByUsername(string username);
        Task<Account?> GetAccountByEmail(string email);
    }
    public class AccountService : IAccountService
    {
        private readonly Context _database;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountService(Context database, IHttpContextAccessor contextAccessor)
        {
            _database = database;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Registers an account into the DB.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        public async Task<Account> Create(RegisterModel registerModel)
        {
            // Hash password
            registerModel.Password = Utilities.HashPassword(registerModel.Password);

            var account = new Account(registerModel);
            await _database.Accounts.AddAsync(account);
            await _database.SaveChangesAsync();
            return account;
        }

        public Task<Account?> GetAccountByUsername(string username)
        {
            return _database.Accounts.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task<Account?> GetAccountByEmail(string email)
        {
            return _database.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
