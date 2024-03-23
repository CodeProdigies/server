using Microsoft.EntityFrameworkCore;
using prod_server.database;
using prod_server.Entities;

namespace prod_server.Services.DB
{

    public interface INotificationsService
    {
        public Task<Notification> Create(Notification notification);
        public Task<bool> MarkAsRead(Guid id);
    }
    public class NotificationsService
    {
        private readonly Context _database;
        private readonly IHttpContextAccessor _contextAccessor;

        public NotificationsService(Context database, IHttpContextAccessor contextAccessor)
        {
            _database = database;
            _contextAccessor = contextAccessor;
        }

        public async Task<Notification> Create(Notification notification)
        {

            await _database.Notifications.AddAsync(notification);
            await _database.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> MarkAsRead(Guid id)
        {
            var notification = await _database.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (notification == null) return false;

            notification.Read = true;
            notification.ReadAt = DateTime.UtcNow;

            await _database.SaveChangesAsync();
            return true;
        }   
    }
}
