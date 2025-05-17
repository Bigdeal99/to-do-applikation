namespace SecureTodoApi.Models.DTOs
{
    public class TodoUpdateRequest
    {
        public string Title { get; set; }= string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool IsCompleted { get; set; }
    }
}
