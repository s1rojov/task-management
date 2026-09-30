using System.ComponentModel.DataAnnotations.Schema;
using Task_management.Models;

[Table("tasks")]
public class TaskModel
{
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public string Title { get; set; } = string.Empty;
    [Column("description")]
    public string? Description { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [Column("status_id")]
    public int StatusId { get; set; }
    public StatusModel Status { get; set; } = null!;
    [Column("user_id")]
    public int UserId { get; set; }
    public UserModel User { get; set; } = null!;
}