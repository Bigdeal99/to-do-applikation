using SecureTodoApi.Models;
using SecureTodoApi.Data;

namespace SecureTodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TodoItem> GetAllByUserId(int userId)
        {
            return _context.TodoItems
                .Where(t => t.UserId == userId)
                .ToList();
        }
    public List<TodoItem> GetByCategory(int userId, string category)
        {
            return _context.TodoItems
                .Where(t => t.UserId == userId && t.Category != null && 
                    t.Category.ToLower() == category.ToLower())
                .ToList();
        }

        public TodoItem? GetById(int id)
        {
            return _context.TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public void Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();
        }

        public void Update(TodoItem item)
        {
            _context.TodoItems.Update(item);
            _context.SaveChanges();
        }

        public void Delete(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
        }
    }
}
