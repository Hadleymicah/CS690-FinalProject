namespace TaskTracker;

using System.Text;

public class ConsoleUI
{
    private TaskManager taskManager;

    public ConsoleUI(TaskManager taskManager)
    {
        this.taskManager = taskManager;
    }

    // Displays the main menu options to the user.
    public void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine("          T A S K T R A C K E R          ");
        Console.WriteLine("=========================================");
        Console.WriteLine($"Today: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}");
        Console.WriteLine();
        Console.WriteLine("[1] Add New Task");
        Console.WriteLine("[2] View Dashboard");
        Console.WriteLine("[3] Edit Task");
        Console.WriteLine("[4] Delete Task");
        Console.WriteLine("[5] Daily Summary");
        Console.WriteLine("[6] Exit Application");
        Console.WriteLine("===================================");
        Console.Write("Enter your choice (1-6): ");
    }

    // Displays the task addition options to the user.

    public void DisplayAddTaskScreen()
    {
        bool continueAdding = true;

        while (continueAdding)
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("          ADD NEW TASK            ");
            Console.WriteLine("===================================");
            
            var newTask = GetTaskInputFromUser();
            
            if (newTask != null)
            {
                Console.WriteLine("\nTask Summary:");
                Console.WriteLine($"Title: {newTask.Title}");
                Console.WriteLine($"Description: {newTask.Description}");
                Console.WriteLine($"Priority: {newTask.TaskPriority}");
                Console.WriteLine($"Important: {(newTask.IsImportant ? "Yes" : "No")}");
                Console.WriteLine($"Due Date: {(newTask.DueDate.HasValue ? newTask.DueDate.Value.ToShortDateString() : "Not set")}");
                
                Console.WriteLine("\n[1] Save task");
                Console.WriteLine("[2] Edit information");
                Console.WriteLine("[3] Cancel");
                Console.Write("Enter your choice (1-3): ");
                
                string confirmation = Console.ReadLine() ?? string.Empty;
                
                if (confirmation == "1")
                {
                    taskManager.AddTask(newTask.Title, newTask.Description, newTask.TaskPriority, newTask.IsImportant, newTask.DueDate);
                    ShowNotification("Task saved successfully!");
                    
                    Console.WriteLine("\n[1] Add another task");
                    Console.WriteLine("[2] Return to main menu");
                    Console.Write("Enter your choice (1-2): ");
                    
                    string addMore = Console.ReadLine() ?? string.Empty;
                    continueAdding = (addMore == "1");
                }
                else if (confirmation == "2")
                {
                    // Stay in the loop to edit information
                    continue;
                }
                else
                {
                    // Cancel and return to main menu
                    continueAdding = false;
                }
            }
            else
            {
                continueAdding = false;
            }
        }
    }


    // Displays the edit task options to the user.
    public void DisplayEditTaskScreen(Task task)
    {
        bool continueEditing = true;
        Task editedTask = new Task(task.Id, task.Title, task.Description, task.TaskPriority, task.IsImportant, task.DueDate)
        {
            IsCompleted = task.IsCompleted,
            CreationDate = task.CreationDate
        };

        while (continueEditing)
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("           EDIT TASK              ");
            Console.WriteLine("===================================");
            Console.WriteLine($"Task ID: {editedTask.Id}");
            Console.WriteLine($"Current Title: {editedTask.Title}");
            Console.WriteLine($"Current Description: {editedTask.Description}");
            Console.WriteLine($"Current Priority: {editedTask.TaskPriority}");
            Console.WriteLine($"Current Importance: {(editedTask.IsImportant ? "Important" : "Not Important")}");
            Console.WriteLine($"Current Due Date: {(editedTask.DueDate.HasValue ? editedTask.DueDate.Value.ToShortDateString() : "Not set")}");
            Console.WriteLine($"Current Status: {(editedTask.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine();
            
            Console.WriteLine("Select field to edit:");
            Console.WriteLine("[1] Title");
            Console.WriteLine("[2] Description");
            Console.WriteLine("[3] Priority");
            Console.WriteLine("[4] Importance");
            Console.WriteLine("[5] Due Date");
            Console.WriteLine("[6] Completion Status");
            Console.WriteLine("[7] Save Changes");
            Console.WriteLine("[8] Cancel");
            Console.Write("Enter your choice (1-8): ");
            
            string choice = Console.ReadLine() ?? string.Empty;
            
            switch (choice)
            {
                case "1":
                    Console.Write("Enter new title: ");
                    string newTitle = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(newTitle))
                    {
                        editedTask.UpdateTitle(newTitle);
                    }
                    break;
                    
                case "2":
                    Console.Write("Enter new description: ");
                    string newDescription = Console.ReadLine() ?? string.Empty;
                    editedTask.UpdateDescription(newDescription ?? "");
                    break;
                    
                case "3":
                    Console.WriteLine("Select new priority:");
                    Console.WriteLine("[1] High");
                    Console.WriteLine("[2] Medium");
                    Console.WriteLine("[3] Low");
                    Console.Write("Enter your choice (1-3): ");
                    string priorityChoice = Console.ReadLine() ?? string.Empty;
                    
                    Priority newPriority = Priority.Medium;
                    if (priorityChoice == "1") newPriority = Priority.High;
                    else if (priorityChoice == "2") newPriority = Priority.Medium;
                    else if (priorityChoice == "3") newPriority = Priority.Low;
                    
                    editedTask.UpdatePriority(newPriority);
                    break;
                    
                case "4":
                    editedTask.ToggleImportance();
                    Console.WriteLine($"Task is now {(editedTask.IsImportant ? "marked as important" : "not marked as important")}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                    
                case "5":
                    Console.WriteLine("Enter new due date (MM/DD/YYYY) or leave empty to remove:");
                    string dueDateInput = Console.ReadLine() ?? string.Empty;
                    
                    if (string.IsNullOrWhiteSpace(dueDateInput))
                    {
                        editedTask.UpdateDueDate(null);
                    }
                    else
                    {
                        try
                        {
                            DateTime dueDate = DateTime.Parse(dueDateInput);
                            editedTask.UpdateDueDate(dueDate);
                        }
                        catch
                        {
                            ShowError("Invalid date format. Due date not updated.");
                        }
                    }
                    break;
                    
                case "6":
                    editedTask.IsCompleted = !editedTask.IsCompleted;
                    Console.WriteLine($"Task is now {(editedTask.IsCompleted ? "completed" : "pending")}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                    
                case "7":
                    if (taskManager.UpdateTask(editedTask))
                    {
                        ShowNotification("Task updated successfully!");
                    }
                    else
                    {
                        ShowError("Failed to update task.");
                    }
                    continueEditing = false;
                    break;
                    
                case "8":
                    continueEditing = false;
                    break;
                    
                default:
                    ShowError("Invalid choice. Please try again.");
                    break;
            }
        }
    }


    // Displays the delete task options and confirmation prompts (NFR5) to the user.
    public void DisplayDeleteTaskConfirmation(Task task)
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("          DELETE TASK             ");
        Console.WriteLine("===================================");
        Console.WriteLine("Are you sure you want to delete this task?");
        Console.WriteLine();
        Console.WriteLine($"Title: {task.Title}");
        Console.WriteLine($"Description: {task.Description}");
        Console.WriteLine($"Priority: {task.TaskPriority}");
        Console.WriteLine($"Due Date: {(task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "Not set")}");
        Console.WriteLine();
        
        if (GetConfirmationFromUser("Delete this task? (y/n): "))
        {
            if (taskManager.RemoveTask(task.Id))
            {
                ShowNotification("Task deleted successfully!");
            }
            else
            {
                ShowError("Failed to delete task.");
            }
        }
    }

    // Displays the dashboard to the user.
    public void DisplayDashboard()
    {
        bool viewingDashboard = true;
        
        while (viewingDashboard)
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("          TASK DASHBOARD          ");
            Console.WriteLine("===================================");
            
            var allTasks = taskManager.GetAllTasks();
            var completedTasks = taskManager.GetCompletedTasks();
            var pendingTasks = taskManager.GetPendingTasks();
            var highPriorityTasks = taskManager.GetTasksByPriority(Priority.High);
            var importantTasks = taskManager.GetImportantTasks();
            
            Console.WriteLine($"Total tasks: {allTasks.Count}");
            Console.WriteLine($"Completed: {completedTasks.Count}");
            Console.WriteLine($"Pending: {pendingTasks.Count}");
            Console.WriteLine($"High Priority: {highPriorityTasks.Count}");
            Console.WriteLine($"Important: {importantTasks.Count}");
            Console.WriteLine();
            
            Console.WriteLine("Filter options:");
            Console.WriteLine("[1] All Tasks");
            Console.WriteLine("[2] Pending Tasks");
            Console.WriteLine("[3] Completed Tasks");
            Console.WriteLine("[4] High Priority Tasks");
            Console.WriteLine("[5] Important Tasks");
            Console.WriteLine("[6] Return to Main Menu");
            Console.Write("Enter your choice (1-6): ");
            
            string filterChoice = Console.ReadLine() ?? string.Empty;
            List<Task> filteredTasks = new List<Task>();
            
            switch (filterChoice)
            {
                case "1":
                    filteredTasks = allTasks;
                    break;
                case "2":
                    filteredTasks = pendingTasks;
                    break;
                case "3":
                    filteredTasks = completedTasks;
                    break;
                case "4":
                    filteredTasks = highPriorityTasks;
                    break;
                case "5":
                    filteredTasks = importantTasks;
                    break;
                case "6":
                    viewingDashboard = false;
                    continue;
                default:
                    ShowError("Invalid choice. Showing all tasks.");
                    filteredTasks = allTasks;
                    break;
            }
            
            if (filteredTasks.Count == 0)
            {
                Console.WriteLine("\nNo tasks to display with this filter.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            DisplayTaskTable(filteredTasks);
            
            Console.WriteLine("\nOptions:");
            Console.WriteLine("Enter task ID to view details/actions");
            Console.WriteLine("Enter 'B' to go back to filter selection");
            Console.WriteLine("Enter 'M' to return to main menu");
            Console.Write("Your choice: ");
            
            string taskChoice = Console.ReadLine() ?? string.Empty;
            
            if (taskChoice.ToUpper() == "B")
            {
                // Continue the loop to see filters again
                continue;
            }
            else if (taskChoice.ToUpper() == "M")
            {
                viewingDashboard = false;
            }
            else if (int.TryParse(taskChoice, out int taskId))
            {
                var task = taskManager.GetTaskById(taskId);
                if (task != null)
                {
                    DisplayTaskDetails(task);
                }
                else
                {
                    ShowError("Task not found.");
                }
            }
            else
            {
                ShowError("Invalid choice.");
            }
        }
    }


    // Displays the task details and actions to the user.
    public void DisplayTaskDetails(Task task)
    {
        bool viewingDetails = true;
        
        while (viewingDetails)
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("          TASK DETAILS            ");
            Console.WriteLine("===================================");
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Priority: {task.TaskPriority}");
            Console.WriteLine($"Important: {(task.IsImportant ? "Yes" : "No")}");
            Console.WriteLine($"Due Date: {(task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "Not set")}");
            Console.WriteLine($"Status: {(task.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine($"Created: {task.CreationDate.ToShortDateString()}");
            Console.WriteLine();
            
            Console.WriteLine("Available actions:");
            Console.WriteLine($"[1] Mark as {(task.IsCompleted ? "Incomplete" : "Complete")}");
            Console.WriteLine("[2] Edit Task");
            Console.WriteLine("[3] Delete Task");
            Console.WriteLine("[4] Return to Dashboard");
            Console.Write("Enter your choice (1-4): ");
            
            string actionChoice = Console.ReadLine() ?? string.Empty;
            
            switch (actionChoice)
            {
                case "1":
                    task.IsCompleted = !task.IsCompleted;
                    taskManager.UpdateTask(task);
                    ShowNotification($"Task marked as {(task.IsCompleted ? "completed" : "pending")}!");
                    break;
                    
                case "2":
                    DisplayEditTaskScreen(task);
                    // Reload task from manager as it might have changed
                    task = taskManager.GetTaskById(task.Id);
                    break;
                    
                case "3":
                    DisplayDeleteTaskConfirmation(task);
                    viewingDetails = false;
                    break;
                    
                case "4":
                    viewingDetails = false;
                    break;
                    
                default:
                    ShowError("Invalid choice. Please try again.");
                    break;
            }
        }
    }


 
    // Displays the daily summary to the user.
    public void DisplayDailySummary(DailySummary summary)
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("          DAILY SUMMARY           ");
        Console.WriteLine("===================================");
        
        Console.WriteLine(summary.GenerateSummaryReport());
        
        Console.WriteLine("\nOptions:");
        Console.WriteLine("[1] Save summary to file");
        Console.WriteLine("[2] Return to Main Menu");
        Console.Write("Enter your choice (1-2): ");
        
        string choice = Console.ReadLine() ?? string.Empty;
        
        if (choice == "1")
        {
            string fileName = $"TaskSummary_{DateTime.Now:yyyyMMdd}.txt";
            File.WriteAllText(fileName, summary.GenerateSummaryReport());
            ShowNotification($"Summary saved to file: {fileName}");
        }
    }


    // Displays the task input options to the user and returns a new Task object.
    public Task? GetTaskInputFromUser()
    {
        Console.Write("Enter task title: ");
        string title = Console.ReadLine() ?? string.Empty;
        
        if (string.IsNullOrWhiteSpace(title))
        {
            ShowError("Title cannot be empty.");
            return null; // Explicitly returning null for invalid input
        }
        
        Console.Write("Enter task description (optional): ");
        string description = Console.ReadLine() ?? "";
        
        Console.WriteLine("\nSelect priority:");
        Console.WriteLine("[1] High");
        Console.WriteLine("[2] Medium");
        Console.WriteLine("[3] Low");
        Console.Write("Enter your choice (1-3): ");
        string priorityInput = Console.ReadLine() ?? string.Empty;
        
        Priority priority = Priority.Medium; // Default
        switch (priorityInput)
        {
            case "1":
                priority = Priority.High;
                break;
            case "2":
                priority = Priority.Medium;
                break;
            case "3":
                priority = Priority.Low;
                break;
            default:
                Console.WriteLine("Invalid priority. Setting to Medium.");
                break;
        }
        
        Console.Write("\nMark as important? (y/n): ");
        bool isImportant = Console.ReadLine()?.ToLower() == "y";
        
        DateTime? dueDate = null;
        Console.Write("\nEnter due date (MM/DD/YYYY) or leave empty for no due date: ");
        string dueDateInput = Console.ReadLine() ?? string.Empty;
        
        if (!string.IsNullOrWhiteSpace(dueDateInput))
        {
            try
            {
                dueDate = DateTime.Parse(dueDateInput);
            }
            catch
            {
                ShowError("Invalid date format. Due date will not be set.");
            }
        }
        
        return new Task(0, title, description, priority, isImportant, dueDate);
    }

    public Task GetTaskUpdatesFromUser(Task currentTask)
    {
        
        return currentTask;
    }

    public int GetTaskIdFromUser()
    {
        Console.Write("Enter task ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            return id;
        }
        
        ShowError("Invalid ID format.");
        return -1;
    }

    public bool GetConfirmationFromUser(string message)
    {
        Console.Write(message);
        string response = Console.ReadLine()?.ToLower() ?? "";
        return response == "y" || response == "yes";
    }

    public void ShowNotification(string message)
    {
        Console.WriteLine();
        Console.WriteLine($">>> {message}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public void ShowError(string message)
    {
        Console.WriteLine();
        Console.WriteLine($"Error: {message}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private void DisplayTaskTable(List<Task> tasks)
    {
        Console.WriteLine();
        Console.WriteLine("ID | Title                  | Priority | Important | Due Date   | Status");
        Console.WriteLine("---+------------------------+----------+-----------+------------+----------");
        
        foreach (var task in tasks)
        {
            string title = task.Title.Length > 20 ? task.Title.Substring(0, 17) + "..." : task.Title.PadRight(20);
            string priority = task.TaskPriority.ToString().PadRight(8);
            string important = (task.IsImportant ? "Yes" : "No").PadRight(9);
            string dueDate = (task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "-").PadRight(10);
            string status = task.IsCompleted ? "Completed" : "Pending";
            
            Console.WriteLine($"{task.Id.ToString().PadRight(2)} | {title} | {priority} | {important} | {dueDate} | {status}");
        }
    }
}
