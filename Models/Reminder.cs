namespace Reminders.Models;

public class Reminder
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Notes { get; set; }
    public DateTime? DueAt { get; set; }
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; }
}
