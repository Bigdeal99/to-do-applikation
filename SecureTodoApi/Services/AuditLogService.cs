using Microsoft.EntityFrameworkCore;
using SecureTodoApi.Data;
using SecureTodoApi.Models;

namespace SecureTodoApi.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService(ApplicationDbContext context, ILogger<AuditLogService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogActionAsync(
            string actionType,
            int? userId,
            string username,
            string ipAddress,
            string userAgent,
            string entityType,
            int? entityId,
            string actionDetails,
            bool isSuccessful = true,
            string? errorMessage = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    ActionType = actionType,
                    UserId = userId,
                    Username = username,
                    Timestamp = DateTime.UtcNow,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    EntityType = entityType,
                    EntityId = entityId,
                    ActionDetails = actionDetails,
                    IsSuccessful = isSuccessful,
                    ErrorMessage = errorMessage
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log entry");
                throw;
            }
        }

        public async Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(int userId)
        {
            return await _context.AuditLogs
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(string entityType, int entityId)
        {
            return await _context.AuditLogs
                .Where(log => log.EntityType == entityType && log.EntityId == entityId)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetRecentAuditLogsAsync(int count = 100)
        {
            return await _context.AuditLogs
                .OrderByDescending(log => log.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}