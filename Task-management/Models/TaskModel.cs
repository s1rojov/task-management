namespace Task_management.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int StatusId { get; set; }
        public int UserId { get; set; }

        public StatusModel Statuses { get; set; } = null!;
        public UserModel Users { get; set; } = null!;

    }
}
