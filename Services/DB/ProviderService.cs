using Microsoft.EntityFrameworkCore;
using prod_server.database;
using prod_server.Entities;

namespace prod_server.Services.DB
{
    public interface IProviderService
    {
        Task<Provider?> Create(Provider provider);
        Task<Provider?> Get(int id);
        Task<int> Update(Provider provider);
        Task<int> Delete(int id);
        Task<List<Provider>> GetAll();
    }
    public class ProviderService : IProviderService
    {   
        private readonly Context _database;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProviderService(Context database, IHttpContextAccessor contextAccessor)
        {
            _database = database;
            _contextAccessor = contextAccessor;
        }

        public async Task<Provider> Create(Provider provider)
        {
            await _database.Providers.AddAsync(provider);
            await _database.SaveChangesAsync();
            return provider;
        }

        public Task<Provider?> Get(int id)
        {
            return _database.Providers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Provider>> GetAll()
        {
            return _database.Providers.ToListAsync();
        }

        public async Task<int> Update(Provider provider)
        {
            _database.Providers.Update(provider);
            return await _database.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var provider = await Get(id);
            if (provider == null)
            {
                return 0;
            }
            _database.Providers.Remove(provider);
            return await _database.SaveChangesAsync();
        }
    }
}
