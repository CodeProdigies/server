using Microsoft.EntityFrameworkCore;
using prod_server.Classes.Others;
using prod_server.database;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace prod_server.Services
{

    public interface IService<TEntity> where TEntity : class
    {
        Task<PagedResult<TEntity>> Search(GenericSearchFilter request);
    }

    public class Service<TEntity> where TEntity : class
    {
        protected readonly Context _database;
        protected readonly IHttpContextAccessor _contextAccessor;

        public Service(Context database, IHttpContextAccessor contextAccessor)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _contextAccessor = contextAccessor;
        }

        public virtual async Task<PagedResult<TEntity>> Search(GenericSearchFilter request)
        {
            try
            {
                IQueryable<TEntity> query = _database.Set<TEntity>();

                foreach (var include in request.Includes)
                {
                    query = query.Include(include);
                }

                foreach (var (propertyName, searchTerm) in request.Filters)
                {
                    try
                    {
                        query = ApplyFilter(query, propertyName, searchTerm);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exception occurred during filtering
                        Console.WriteLine($"Error applying filter for property '{propertyName}': {ex.Message}");
                    }
                }

                var count = await query.CountAsync();
                query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

                var items = await query.ToListAsync();

                return new PagedResult<TEntity>(items, request.Page, request.PageSize, count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for {typeof(TEntity).Name}: {ex.Message}");
                return new PagedResult<TEntity>(new List<TEntity>(), 1, 10, 0);
            }
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        private IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, string propertyName, string searchTerm)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, propertyName);
            var propertyType = typeof(TEntity).GetProperty(propertyName)?.PropertyType;

            if (propertyType == null)
            {
                // If the property type is not found, return the original query
                return query;
            }

            // Handle string properties separately to allow case-insensitive search
            if (propertyType == typeof(string))
            {
                // Convert property value and search term to lowercase
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var propertyToLower = Expression.Call(property, toLowerMethod);
                var searchTermToLower = Expression.Constant(searchTerm.ToLower());

                // Apply Contains method on the lowercase property value
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var containsCall = Expression.Call(propertyToLower, containsMethod, searchTermToLower);

                // Create lambda expression
                var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
                return query.Where(lambda);
            }
            else
            {
                // Handle nullable types
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                // Attempt to cast the search term to the underlying property type
                try
                {
                    var searchTermValue = Convert.ChangeType(searchTerm, underlyingType);
                    var constant = Expression.Constant(searchTermValue, underlyingType);

                    // If the property is nullable, check if it has a value and compare it to the search term
                    if (underlyingType != propertyType)
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var value = Expression.Property(property, "Value");

                        var equalsMethod = underlyingType.GetMethod("Equals", new[] { underlyingType });
                        var equalsCall = Expression.Call(value, equalsMethod, constant);

                        var notNullAndEquals = Expression.AndAlso(hasValue, equalsCall);
                        var lambda = Expression.Lambda<Func<TEntity, bool>>(notNullAndEquals, parameter);

                        return query.Where(lambda);
                    }
                    else
                    {
                        var equalsMethod = underlyingType.GetMethod("Equals", new[] { underlyingType });
                        var equalsCall = Expression.Call(property, equalsMethod, constant);

                        var lambda = Expression.Lambda<Func<TEntity, bool>>(equalsCall, parameter);
                        return query.Where(lambda);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exception occurred during the casting
                    Console.WriteLine($"Error casting search term '{searchTerm}' to type '{propertyType.Name}': {ex.Message}");
                    return query;
                }
            }
        }


    }
}
