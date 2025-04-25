namespace TaskTracker;

using System.Text;

public class DailySummary
{
    public DateTime Date { get; set; }
    public int CompletedTasksCount { get; set; }
    public int PendingTasksCount { get; set; }
    public List<Task> CompletedTasks { get; set; }
    public List<Task> PendingTasks { get; set; }


    // Constructor to initialize the DailySummary with the date, completed tasks, and pending tasks.
    public DailySummary(DateTime date, List<Task> completedTasks, List<Task> pendingTasks)
    {
        Date = date;
        CompletedTasks = completedTasks;
        PendingTasks = pendingTasks;
        CompletedTasksCount = completedTasks.Count;
        PendingTasksCount = pendingTasks.Count;
    }


    // FR5: Generates an end-of-day summary report of completed and pending tasks.
    public string GenerateSummaryReport()
    {
        StringBuilder report = new StringBuilder();
        report.AppendLine($"Daily Summary for {Date.ToShortDateString()}");
        report.AppendLine($"Tasks completed today: {CompletedTasksCount}");
        report.AppendLine($"Tasks pending: {PendingTasksCount}");
        
        // Display upcoming due dates (tasks due in the next 3 days).
        report.AppendLine("\nUpcoming due dates (next 3 days):");
        var upcomingTasks = PendingTasks
            .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date <= DateTime.Now.AddDays(3).Date)
            .OrderBy(t => t.DueDate)
            .ToList();
            
        if (upcomingTasks.Count > 0)
        {
            foreach (var task in upcomingTasks)
            {
                report.AppendLine($"- {task.Title} (Due: {task.DueDate?.ToShortDateString()})");
            }
        }
        else
        {
            report.AppendLine("- No upcoming tasks due in the next 3 days");
        }
        
        // List completed tasks
        report.AppendLine("\nCompleted tasks:");
        if (CompletedTasks.Count > 0)
        {
            foreach (var task in CompletedTasks)
            {
                report.AppendLine($"- {task.Title} (Priority: {task.TaskPriority})");
            }
        }
        else
        {
            report.AppendLine("- No tasks completed today");
        }
        
        // List pending tasks
        report.AppendLine("\nPending tasks:");
        if (PendingTasks.Count > 0)
        {
            foreach (var task in PendingTasks)
            {
                string dueInfo = task.DueDate.HasValue ? $"Due: {task.DueDate.Value.ToShortDateString()}" : "No due date";
                report.AppendLine($"- {task.Title} (Priority: {task.TaskPriority}, {dueInfo})");
            }
        }
        else
        {
            report.AppendLine("- No pending tasks");
        }
        
        return report.ToString();
    }
}
