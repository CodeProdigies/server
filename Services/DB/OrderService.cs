using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IOrderService : IService<Order>
    {
        public Task<Order> Create(Order quote);
        public Task<List<Order>> GetAll();
        public Task<Order?> Get(int id);
        public Task<List<Order>> GetFromCustomerId(int customerId);
        public Task Delete(int id);
        public Task Update(Order order);
        public Task<List<Order>> GetByCustomer(int id);
    }

    public class OrderService : Service<Order>, IOrderService
    {
        public OrderService(Context database, IHttpContextAccessor contextAccessor) : base(database, contextAccessor) { }

        async public Task<Order> Create(Order order)
        {
            await _database.Orders.AddAsync(order);
            await _database.SaveChangesAsync();
            return order;
        }

        public Task<List<Order>> GetAll()
        {
            return _database.Orders.OrderByDescending(q => q.CreatedAt).ToListAsync();
        }

        private IQueryable<Order> GetOrderWithProducts()
        {
            return _database.Orders
                    .Include(o => o.Products)
                    .ThenInclude(cp => cp.Product)
                    .Where(b => b.isArchived == false);
        }

        public Task<Order?> Get(int id)
        {
            _database.ChangeTracker.LazyLoadingEnabled = false;
            return  GetOrderWithProducts()
                    .FirstOrDefaultAsync(o => o.Id == id);
        }

        public Task<List<Order>> GetFromCustomerId(int customerId)
        {
            _database.ChangeTracker.LazyLoadingEnabled = false;
            return GetOrderWithProducts().Where(q => q.CustomerId == customerId).ToListAsync();
        }


        async public Task Delete(int id)
        {
            var order = _database.Orders.FirstOrDefault(order => order.Id == id);
            if (order == null) throw new Exception("Order not found.");

            // Check if customer has any relationships with other entities
            var relatedEntities = _database.Entry(order)
                .Metadata
                .GetNavigations()
                .Where(n => !n.IsCollection)
                .Select(n => _database.Entry(order)?.Reference(n.Name)?.TargetEntry?.Entity)
                .ToList();

            if (relatedEntities.Any())
            {
                // Customer has related entities, handle accordingly (throw exception, log, etc.)
                throw new InvalidOperationException("Order cannot be deleted because it has related entities.");
            }

            _database.Remove(order);
            await _database.SaveChangesAsync();
        }

        async public Task Update(Order order)
        {
            order.Products.ForEach(p =>
            {
                p.Total = p.Quantity * p.SellPrice;
            });

            order.Total = order.Products.Sum(p => p.Total);
            _database.Orders.Update(order);
            await _database.SaveChangesAsync();
        }

        async public Task<List<Order>> GetByCustomer(int id)
        {
            return await _database.Orders.Where(o => o.CustomerId == id && o.isArchived == false).ToListAsync();
        }

    }
}
