using Microsoft.EntityFrameworkCore;
using prod_server.database;
using prod_server.Entities;
using prod_server.Migrations;

namespace prod_server.Services.DB
{

    public interface INotificationsService
    {
        public Task<Notification> Create(Notification notification);
        public Task<bool> MarkAsRead(Guid id);
        public Task<Notification?> Get(Guid id);
        public Task<int> Delete(Guid id);
    }
    public class NotificationsService : INotificationsService
    {
        private readonly Context _database;

        public NotificationsService(Context database)
        {
            _database = database;
        }

        public async Task<Notification> Create(Notification notification)
        {

            await _database.Notifications.AddAsync(notification);
            await _database.SaveChangesAsync();
            return notification;
        }
        public async Task<Notification?> Get(Guid id)
        {
            return await _database.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<int> Delete(Guid id)
        {
            var notificationToDelete = await _database.Notifications.FindAsync(id);

            if (notificationToDelete != null)
            {
                _database.Notifications.Remove(notificationToDelete);
                return await _database.SaveChangesAsync();
            }
            return 0; // Return 0 if the product with the specified id is not found
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
