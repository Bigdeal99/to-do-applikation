using SecureTodoApi.Models;

namespace SecureTodoApi.Services
{
    public interface IAuditLogService
    {
        Task LogActionAsync(
            string actionType,
            int? userId,
            string username,
            string ipAddress,
            string userAgent,
            string entityType,
            int? entityId,
            string actionDetails,
            bool isSuccessful = true,
            string? errorMessage = null
        );

        Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(int userId);
        Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(string entityType, int entityId);
        Task<IEnumerable<AuditLog>> GetRecentAuditLogsAsync(int count = 100);
    }
}