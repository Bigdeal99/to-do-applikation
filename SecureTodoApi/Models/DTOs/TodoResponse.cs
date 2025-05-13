namespace SecureTodoApi.Models.DTOs
{
    public class TodoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
