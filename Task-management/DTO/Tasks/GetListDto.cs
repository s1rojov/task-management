namespace Task_management.DTOs.Tasks
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        //public TaskStatusDto Status { get; set; } = null!;
        //public TaskUserDto User { get; set; } = null!;
    }

    public class TaskStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class TaskUserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}