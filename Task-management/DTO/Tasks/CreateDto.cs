using System.ComponentModel.DataAnnotations;

public class CreateTaskDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public int StatusId { get; set; }

    [Required]
    public int UserId { get; set; }
}