using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace prod_server.Entities
{
    public class Notification
    {
        public Notification() { }
        public Notification (string title, string message, NotificationType type)
        {
            Subject = title;
            Message = message;
            Type = type;
        }
        public Notification (Account account)
        {
            Subject = "Welcome to the platform!";
            Message = "Welcome. If you have any questions, please reach out to us at support@josephsthebest.com";
            Type = NotificationType.Info;
            UserId = account.Id;
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("subject")]
        public string Subject { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("type")]
        public NotificationType Type { get; set; }
        [Column("linkUrl")]
        public string? LinkUrl { get; set; }
        [Column("read")]
        public bool Read { get; set; } = false;
        [Column("read_date")]
        public DateTime? ReadAt { get; set; }
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        // Foreign key for User
        [Column("userId")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Account User { get; set; }
    }   
    public enum NotificationType
    {
        Info,
        Warning,
        Error
    }
}

