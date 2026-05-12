namespace TaskTracker.API.Models;

public enum TaskStatus { ToDo, InProgress, Done }
public enum Priority { Low, Medium, High }

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? Deadline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}