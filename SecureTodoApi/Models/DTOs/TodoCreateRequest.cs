namespace SecureTodoApi.Models.DTOs
{
    public class TodoCreateRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
