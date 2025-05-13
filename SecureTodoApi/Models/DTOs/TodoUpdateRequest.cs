namespace SecureTodoApi.Models.DTOs
{
    public class TodoUpdateRequest
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
