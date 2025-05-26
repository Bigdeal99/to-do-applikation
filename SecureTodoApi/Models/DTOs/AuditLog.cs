using System;

namespace SecureTodoApi.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string ActionType { get; set; } = string.Empty; 
        public int? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty; 
        public int? EntityId { get; set; }
        public string ActionDetails { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}