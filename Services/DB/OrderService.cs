using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.database;
using prod_server.Entities;
using System.Security.Claims;

namespace prod_server.Services.DB
{
    public interface IOrderService
    {
        public Task<Order> Create(Order quote);
        public Task<List<Order>> GetAll();
        public Task<Order?> Get(int id);
        public Task Delete(int id);
        public Task Update(Order order);
    }

    public class OrderService : IOrderService
    {
        private readonly Context _database;

        public OrderService(Context database)
        {
            _database = database;
        }

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
                .Select(q => new Order
                {
                    Id = q.Id,
                    CreatedAt = q.CreatedAt,
                    CustomerId = q.CustomerId,
                    Customer = q.Customer,
                    // Include other Quote properties here...
                    Products = q.Products.Select(cp => new OrderItem
                    {
                        Id = cp.Id,
                        ProductId = cp.ProductId,
                        Quantity = cp.Quantity,
                        Product = cp.Product,
                        IsSold = cp.IsSold,
                        Total = cp.Total,
                        OrderId = cp.OrderId,
                        SellPrice = cp.SellPrice,
                        BuyPrice = cp.BuyPrice
                    }).ToList()
                });
        }

        public Task<Order?> Get(int id)
        {
            _database.ChangeTracker.LazyLoadingEnabled = false;
            return GetOrderWithProducts().FirstOrDefaultAsync(q => q.Id == id);
        }


        async public Task Delete(int id)
        {
            var quote = _database.Orders.FirstOrDefault(quote => quote.Id == id);
            if (quote == null) throw new Exception("Quote not found.");

            _database.Remove(quote);
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

    }
}
