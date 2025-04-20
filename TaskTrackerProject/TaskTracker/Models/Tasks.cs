namespace TaskTracker;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public Priority TaskPriority { get; set; }
    public bool IsImportant { get; set; }

    public Task(int id, string title, string description, Priority priority, bool isImportant, DateTime? dueDate)
    {
        Id = id;
        Title = title;
        Description = description;
        CreationDate = DateTime.Now;
        DueDate = dueDate;
        IsCompleted = false;
        TaskPriority = priority;
        IsImportant = isImportant;
    }

    public void MarkAsComplete()
    {
        IsCompleted = true;
    }

    public void UpdatePriority(Priority priority)
    {
        TaskPriority = priority;
    }

    public void ToggleImportance()
    {
        IsImportant = !IsImportant;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public void UpdateDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
    }
}

public enum Priority
{
    Low,
    Medium,
    High
}
