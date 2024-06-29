using Microsoft.EntityFrameworkCore;
using prod_server.database;
using prod_server.Entities;

namespace prod_server.Services.DB
{
    public interface IProviderService : IService<Provider>
    {
        Task<Provider?> Create(Provider provider);
        Task<Provider?> Get(int id);
        Task<int> Update(Provider provider);
        Task<int> Delete(int id);
        Task<List<Provider>> GetAll();
    }
    public class ProviderService : Service<Provider>, IProviderService
    {   
        public ProviderService(Context database, IHttpContextAccessor contextAccessor) : base(database, contextAccessor) { }
        public async Task<Provider> Create(Provider provider)
        {
            await _database.Providers.AddAsync(provider);
            await _database.SaveChangesAsync();
            return provider;
        }

        public Task<Provider?> Get(int id)
        {
            return _database.Providers.Where(x => x.isArchived == false).FirstOrDefaultAsync(x => x.Id == id);
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

            // Check if customer has any relationships with other entities
            var relatedEntities = _database.Entry(provider)
                .Metadata
                .GetNavigations()
                .Where(n => !n.IsCollection)
                .Select(n => _database.Entry(provider)?.Reference(n.Name)?.TargetEntry?.Entity)
                .ToList();

            if (relatedEntities.Any())
            {
                // Customer has related entities, handle accordingly (throw exception, log, etc.)
                throw new InvalidOperationException("Provider cannot be deleted because it has related entities.");
            }
            _database.Providers.Remove(provider);
            return await _database.SaveChangesAsync();
        }
    }
}
