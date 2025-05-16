using SecureTodoApi.Models;

namespace SecureTodoApi.Repositories
{
    public interface ITodoRepository
    {
        List<TodoItem> GetAllByUserId(int userId);
        TodoItem? GetById(int id);
        void Create(TodoItem item);
        void Update(TodoItem item);
        void Delete(TodoItem item);
        List<TodoItem> GetByCategory(int userId, string category);

    }
}
