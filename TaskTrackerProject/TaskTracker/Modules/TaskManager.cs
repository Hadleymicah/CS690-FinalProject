namespace TaskTracker;

using System.IO;
using System.Text;

public class TaskManager
{
    private List<Task> Tasks { get; set; }
    private const string Delimiter = "|||"; // This is used to separate fields when saving tasks to a file.
    private readonly string filePath = "tasks.txt"; // File path for task persistence.
    private int nextId = 1; // Tracks the next available task ID.

    public TaskManager()
    {
        Tasks = new List<Task>();
        LoadTasks(); // NFR3: Tasks are loaded from persistent storage on initialization.
        
    }

    //Overloaded constructor to allow for testing
     public TaskManager(List<Task>? initialTasks = null, string? customFilePath = null)
    {
        if (initialTasks != null)
        {
            Tasks = initialTasks;
        }
        else
        {
            Tasks = new List<Task>();
            filePath = customFilePath ?? "tasks.txt";
            LoadTasks();
        }
    }

    private int GetNextTaskId()
    {
        return nextId++;
    }

    // FR1: Allows adding a new task with required properties.
    public Task AddTask(string title, string description, Priority priority, bool isImportant, DateTime? dueDate)
    {
    if (string.IsNullOrEmpty(title))
    {
        throw new ArgumentNullException(nameof(title), "Title cannot be null or empty.");
    }

    var newTask = new Task(GetNextTaskId(), title, description, priority, isImportant, dueDate);


    Tasks.Add(newTask);
    SaveTasks(); // NFR3: Ensures tasks are saved persistently.
    return newTask;
    }

    // FR6: Allows permanent removal of a task.
    public bool RemoveTask(int id)
    {
        var task = GetTaskById(id);
        if (task != null)
        {
            Tasks.Remove(task);
            SaveTasks(); // NFR3: Updates the persistent data after deletion.
            return true;
        }
        return false;
    }

    public List<Task> GetAllTasks()
    {
        return Tasks;
    }

    // FR3: Retrieves tasks categorized by priority.
    public List<Task> GetTasksByPriority(Priority priority)
    {
        return Tasks.Where(t => t.TaskPriority == priority).ToList();
    }


    // FR3: Retrieves tasks that are completed.
    public List<Task> GetCompletedTasks()
    {
        return Tasks.Where(t => t.IsCompleted).ToList();
    }

    // FR3: Retrieves tasks that are pending (not completed).
    public List<Task> GetPendingTasks()
    {
        return Tasks.Where(t => !t.IsCompleted).ToList();
    }


    // FR2: Retrieves tasks marked as "important."
    public List<Task> GetImportantTasks()
    {
        return Tasks.Where(t => t.IsImportant).ToList();
    }

    public Task GetTaskById(int id)
    {
        return Tasks.FirstOrDefault(t => t.Id == id)!;
    }


    // FR7: Allows modification of a task's properties.
    public bool UpdateTask(Task task)
    {
        var existingTask = GetTaskById(task.Id);
        if (existingTask != null)
        {
            // Update task properties
            existingTask.UpdateTitle(task.Title);
            existingTask.UpdateDescription(task.Description);
            existingTask.UpdatePriority(task.TaskPriority);
            existingTask.UpdateDueDate(task.DueDate);
            if (existingTask.IsImportant != task.IsImportant)
            {
                existingTask.ToggleImportance();
            }
            if (existingTask.IsCompleted != task.IsCompleted)
            {
                existingTask.IsCompleted = task.IsCompleted;
            }
            SaveTasks(); // NFR3: Ensures updated tasks are stored persistently.
            return true;
        }
        return false;
    }



    // Saves all tasks to persistent storage.
    public void SaveTasks()
    {
        var lines = new List<string>();
        foreach (var task in Tasks)
        {
            string dueDateStr = task.DueDate.HasValue ? task.DueDate.Value.ToString("MM/dd/yyyy") : "NoDate";
            string taskLine = string.Join(Delimiter, new[]
            {
                task.Id.ToString(),
                task.Title,
                task.Description,
                task.CreationDate.ToString("MM/dd/yyyy"),
                dueDateStr,
                task.IsCompleted.ToString(),
                task.TaskPriority.ToString(),
                task.IsImportant.ToString()
            });
            lines.Add(taskLine);
        }
        File.WriteAllLines(filePath, lines);
    }


    // Loads tasks from persistent storage.
    public void LoadTasks()
    {
        if (!File.Exists(filePath))
        {
            return; // If the file doesn't exist, there are no tasks to load.
        }

        Tasks.Clear();
        nextId = 1;

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            try
            {
                // Parses task data from the file.
                var parts = line.Split(Delimiter);
                if (parts.Length < 8)
                {
                    Console.WriteLine($"Skipping invalid task: {line}");
                    continue;
                }

                int id = int.Parse(parts[0]);
                string title = parts[1];
                string description = parts[2];
                DateTime creationDate = DateTime.Parse(parts[3]);
                
                DateTime? dueDate = null;
                if (parts[4] != "NoDate")
                {
                    dueDate = DateTime.Parse(parts[4]);
                }
                
                bool isCompleted = bool.Parse(parts[5]);
                Priority priority = Enum.Parse<Priority>(parts[6]);
                bool isImportant = bool.Parse(parts[7]);

                var task = new Task(id, title, description, priority, isImportant, dueDate)
                {
                    CreationDate = creationDate,
                    IsCompleted = isCompleted
                };
                
                Tasks.Add(task);
                
                // Update nextId to be higher than any loaded ID
                if (id >= nextId)
                {
                    nextId = id + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading task: {ex.Message}");
            }
        }
    }
}
