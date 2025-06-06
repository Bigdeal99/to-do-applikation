using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureTodoApi.Models.DTOs;
using SecureTodoApi.Services;
using System.Security.Claims;

namespace SecureTodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public IActionResult GetTodos([FromQuery] string? category, [FromQuery] bool? isCompleted)

        {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

    var userId = int.Parse(userIdClaim);

    if (!string.IsNullOrEmpty(category))
    {
        var todos = _todoService.GetTodosByCategory(userId, category);
        return Ok(todos);
    }

    if (isCompleted.HasValue)
    {
        var todos = _todoService.GetTodosByCompletionStatus(userId, isCompleted.Value);
        return Ok(todos);
    }

    var allTodos = _todoService.GetTodosByUserId(userId);
    return Ok(allTodos);
        }


        [HttpPost]
        public IActionResult CreateTodo([FromBody] TodoCreateRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var todo = _todoService.CreateTodo(userId, request);
            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] TodoUpdateRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var success = _todoService.UpdateTodo(userId, id, request);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = int.Parse(userIdClaim);
            var success = _todoService.DeleteTodo(userId, id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
