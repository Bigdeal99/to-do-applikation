using SecureTodoApi.Models;
using SecureTodoApi.Models.DTOs;
using SecureTodoApi.Repositories;

namespace SecureTodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public List<TodoResponse> GetTodosByUserId(int userId)
        {
            var todos = _todoRepository.GetAllByUserId(userId);
            return todos.Select(t => new TodoResponse
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            }).ToList();
        }

        public TodoResponse CreateTodo(int userId, TodoCreateRequest request)
{
    var todo = new TodoItem
    {
        Title = request.Title,
        Description = request.Description,
        Category = request.Category,
        IsCompleted = false,
        UserId = userId
    };

    _todoRepository.Create(todo);

    return new TodoResponse
    {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description,
        Category = todo.Category,
        IsCompleted = todo.IsCompleted
    };
}

        public bool UpdateTodo(int userId, int todoId, TodoUpdateRequest request)
{
    var todo = _todoRepository.GetById(todoId);
    if (todo == null || todo.UserId != userId) return false;

    todo.Title = request.Title;
    todo.Description = request.Description;
    todo.Category = request.Category;
    todo.IsCompleted = request.IsCompleted;

    _todoRepository.Update(todo);
    return true;
}

        public bool DeleteTodo(int userId, int todoId)
        {
            var todo = _todoRepository.GetById(todoId);
            if (todo == null || todo.UserId != userId) return false;

            _todoRepository.Delete(todo);
            return true;
        }
    }
}
