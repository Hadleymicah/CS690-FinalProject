namespace TaskTracker;

// Represents a task in the task tracking system.
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

    // Constructor to initialize a new task with required properties.
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


    // FR4: Marks the task as completed.
    public void MarkAsComplete()
    {
        IsCompleted = true;
    }


    // FR7: Updates the priority of the task.
    public void UpdatePriority(Priority priority)
    {
        TaskPriority = priority;
    }


    // FR2: Toggles the importance status of the task.
    public void ToggleImportance()
    {
        IsImportant = !IsImportant;
    }


    // FR7: Updates the title of the task.
    public void UpdateTitle(string title)
    {
        Title = title;
    }


    // FR7: Updates the description of the task.
    public void UpdateDescription(string description)
    {
        Description = description;
    }


    // FR7: Updates the due date of the task.
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
