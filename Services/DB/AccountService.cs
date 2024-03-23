using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Linq.Expressions;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IAccountService
    {
        Task<Account?> Create(RegisterModel registerModel);
        Task<Account?> GetByUsername(string username);
        Task<Account?> GetByEmail(string email);
        Task<Account?> GetById(string userId);
        Task<Account?> GetById(int userId);
        Task<int> Update(Account account);
        Task<int> Delete(Guid id);
        Task<List<Account>> GetAll(bool includePassword = false);
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
            await _database.Notifications.AddAsync(new Notification(account));
            await _database.SaveChangesAsync();
            return account;
        }

        public Task<Account?> GetByUsername(string username)
        {
            return _database.Accounts.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task<Account?> GetByEmail(string email)
        {
            return _database.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<List<Account>> GetAll(bool includePassword = false)
        {
            IQueryable<Account> query = _database.Accounts;

            if (!includePassword)
            {
                query = query.Select(ExcludePassword());
            }

            return query.ToListAsync();
        }

        public Task<Account?> GetById(string userId)
        {
           return GetById(int.Parse(userId));
        }
        public Task<Account?> GetById(int userId)
        {
           return _database.Accounts.Include(n => n.Notifications).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<int> Update(Account account)
        {
            // Assuming product is tracked by the context (either attached or loaded)
            _database.Accounts.Update(account);

            return _database.SaveChangesAsync();
        }

        public async Task<int> Delete(Guid id)
        {
            var accountToDelete = await _database.Accounts.FindAsync(id);

            if (accountToDelete != null)
            {
                _database.Accounts.Remove(accountToDelete);
                return await _database.SaveChangesAsync();
            }
            return 0; // Return 0 if the product with the specified id is not found
        }

        private Expression<Func<Account, Account>> ExcludePassword()
        {
            return a => new Account
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                Address = a.Address,
                City = a.City,
                State = a.State,    
                ZipCode = a.ZipCode,
                Notifications = a.Notifications
            };
        }
    }
}
