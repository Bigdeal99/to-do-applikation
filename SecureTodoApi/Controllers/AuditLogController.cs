using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureTodoApi.Services;
using System.Security.Claims;

namespace SecureTodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Only admins can view audit logs
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLogs(int userId)
        {
            var logs = await _auditLogService.GetUserAuditLogsAsync(userId);
            return Ok(logs);
        }

        [HttpGet("entity/{entityType}/{entityId}")]
        public async Task<IActionResult> GetEntityLogs(string entityType, int entityId)
        {
            var logs = await _auditLogService.GetEntityAuditLogsAsync(entityType, entityId);
            return Ok(logs);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentLogs([FromQuery] int count = 100)
        {
            var logs = await _auditLogService.GetRecentAuditLogsAsync(count);
            return Ok(logs);
        }
    }
}