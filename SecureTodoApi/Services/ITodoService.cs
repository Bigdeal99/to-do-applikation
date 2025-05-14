using SecureTodoApi.Models;
using SecureTodoApi.Models.DTOs;

namespace SecureTodoApi.Services
{
    public interface ITodoService
    {
        List<TodoResponse> GetTodosByUserId(int userId);
        TodoResponse CreateTodo(int userId, TodoCreateRequest request);
        bool UpdateTodo(int userId, int todoId, TodoUpdateRequest request);
        bool DeleteTodo(int userId, int todoId);
    }
}
