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
            
            // Get task input from the user
            var newTask = GetTaskInputFromUser();
            
            if (newTask != null)
            {
                // Display a summary of the task entered
                Console.WriteLine("\nTask Summary:");
                Console.WriteLine($"Title: {newTask.Title}");
                Console.WriteLine($"Description: {newTask.Description}");
                Console.WriteLine($"Priority: {newTask.TaskPriority}");
                Console.WriteLine($"Important: {(newTask.IsImportant ? "Yes" : "No")}");
                Console.WriteLine($"Due Date: {(newTask.DueDate.HasValue ? newTask.DueDate.Value.ToShortDateString() : "Not set")}");
                
                // Provide options to save, edit, or cancel the task
                Console.WriteLine("\n[1] Save task");
                Console.WriteLine("[2] Edit information");
                Console.WriteLine("[3] Cancel");
                Console.Write("Enter your choice (1-3): ");
                
                string confirmation = Console.ReadLine() ?? string.Empty;
                
                if (confirmation == "1")
                {
                    // Save the task and ask if the user wants to add another one
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

        // Create a copy of the original task to make edits without modifying the original directly
        Task editedTask = new Task(task.Id, task.Title, task.Description, task.TaskPriority, task.IsImportant, task.DueDate)
        {
            IsCompleted = task.IsCompleted,
            CreationDate = task.CreationDate
        };

        // Loop to display editing options
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
            
            // Display menu for editing specific fields
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
                // Update task title
                    Console.Write("Enter new title: ");
                    string newTitle = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(newTitle))
                    {
                        editedTask.UpdateTitle(newTitle);
                    }
                    break;
                    
                case "2":
                // Update task description
                    Console.Write("Enter new description: ");
                    string newDescription = Console.ReadLine() ?? string.Empty;
                    editedTask.UpdateDescription(newDescription ?? "");
                    break;
                    
                case "3":
                // Update task priority
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
                // Toggle importance status
                    editedTask.ToggleImportance();
                    Console.WriteLine($"Task is now {(editedTask.IsImportant ? "marked as important" : "not marked as important")}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                    
                case "5":
                // Update due date
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
                // Toggle completion status
                    editedTask.IsCompleted = !editedTask.IsCompleted;
                    Console.WriteLine($"Task is now {(editedTask.IsCompleted ? "completed" : "pending")}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                    
                case "7":
                // Save changes and update the task in the task manager
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
                // Cancel editing and return to the previous screen
                    continueEditing = false;
                    break;
                    
                default:
                // Handle invalid input
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
        // Show details of the task being deleted for user confirmation.
        Console.WriteLine("Are you sure you want to delete this task?");
        Console.WriteLine();
        Console.WriteLine($"Title: {task.Title}");
        Console.WriteLine($"Description: {task.Description}");
        Console.WriteLine($"Priority: {task.TaskPriority}");
        Console.WriteLine($"Due Date: {(task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "Not set")}");
        Console.WriteLine();
        
        // Confirm deletion with the user.
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
            
            // Retrieve task statistics for display.
            var allTasks = taskManager.GetAllTasks();
            var completedTasks = taskManager.GetCompletedTasks();
            var pendingTasks = taskManager.GetPendingTasks();
            var highPriorityTasks = taskManager.GetTasksByPriority(Priority.High);
            var importantTasks = taskManager.GetImportantTasks();
            
            // Display statistics to the user.
            Console.WriteLine($"Total tasks: {allTasks.Count}");
            Console.WriteLine($"Completed: {completedTasks.Count}");
            Console.WriteLine($"Pending: {pendingTasks.Count}");
            Console.WriteLine($"High Priority: {highPriorityTasks.Count}");
            Console.WriteLine($"Important: {importantTasks.Count}");
            Console.WriteLine();
            
            // Provide filter options to view specific sets of tasks.
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
            
            // Determine filtering logic based on user input.
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
                    // Return to main menu
                    viewingDashboard = false;
                    continue;
                default:
                    ShowError("Invalid choice. Showing all tasks.");
                    filteredTasks = allTasks;
                    break;
            }

            // Display tasks based on the chosen filter.
            if (filteredTasks.Count == 0)
            {
                Console.WriteLine("\nNo tasks to display with this filter.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            // Render a table of filtered tasks.
            DisplayTaskTable(filteredTasks);
            
            Console.WriteLine("\nOptions:");
            Console.WriteLine("Enter task ID to view details/actions");
            Console.WriteLine("Enter 'B' to go back to filter selection");
            Console.WriteLine("Enter 'M' to return to main menu");
            Console.Write("Your choice: ");
            
            string taskChoice = Console.ReadLine() ?? string.Empty;
            
             // Handle user actions on tasks or dashboard navigation.
            if (taskChoice.ToUpper() == "B")
            {
                // Continue the loop to see filters again
                continue;
            }
            else if (taskChoice.ToUpper() == "M")
            {
                viewingDashboard = false; // Exit to main menu.
            }
            else if (int.TryParse(taskChoice, out int taskId))
            {
                var task = taskManager.GetTaskById(taskId);
                if (task != null)
                {
                    DisplayTaskDetails(task); // Show task details and actions.
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

            // Show detailed task information.
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Priority: {task.TaskPriority}");
            Console.WriteLine($"Important: {(task.IsImportant ? "Yes" : "No")}");
            Console.WriteLine($"Due Date: {(task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "Not set")}");
            Console.WriteLine($"Status: {(task.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine($"Created: {task.CreationDate.ToShortDateString()}");
            Console.WriteLine();
            
            // Provide actions for the task.
            Console.WriteLine("Available actions:");
            Console.WriteLine($"[1] Mark as {(task.IsCompleted ? "Incomplete" : "Complete")}");
            Console.WriteLine("[2] Edit Task");
            Console.WriteLine("[3] Delete Task");
            Console.WriteLine("[4] Return to Dashboard");
            Console.Write("Enter your choice (1-4): ");
            
            string actionChoice = Console.ReadLine() ?? string.Empty;
            
            // Handle user input for task actions.
            switch (actionChoice)
            {
                case "1":
                // Toggle task completion status.
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
                // Open the delete task confirmation screen.
                    DisplayDeleteTaskConfirmation(task);
                    viewingDetails = false;
                    break;
                    
                case "4":
                // Exit task details and return to the dashboard.
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
        
        // Show the summary report generated by the DailySummary object.
        Console.WriteLine(summary.GenerateSummaryReport());
        
        Console.WriteLine("\nOptions:");
        Console.WriteLine("[1] Save summary to file");
        Console.WriteLine("[2] Return to Main Menu");
        Console.Write("Enter your choice (1-2): ");
        
        string choice = Console.ReadLine() ?? string.Empty;

        // Allow the user to save the summary to a file or return to the main menu.
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
        
        // Prompt the user to enter an optional description for the task.
        Console.Write("Enter task description (optional): ");
        string description = Console.ReadLine() ?? "";
        
        // Display priority selection options.
        Console.WriteLine("\nSelect priority:");
        Console.WriteLine("[1] High");
        Console.WriteLine("[2] Medium");
        Console.WriteLine("[3] Low");
        Console.Write("Enter your choice (1-3): ");
        string priorityInput = Console.ReadLine() ?? string.Empty;
        
        // Set the default priority to Medium.
        Priority priority = Priority.Medium; 
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
            // Notify the user of invalid input and retain the default priority.
                Console.WriteLine("Invalid priority. Setting to Medium.");
                break;
        }
        
        // Ask the user whether the task should be marked as important.
        Console.Write("\nMark as important? (y/n): ");
        bool isImportant = Console.ReadLine()?.ToLower() == "y";
        
        // Initialize the due date to null, as it is optional.
        DateTime? dueDate = null;
        Console.Write("\nEnter due date (MM/DD/YYYY) or leave empty for no due date: ");
        string dueDateInput = Console.ReadLine() ?? string.Empty;
        
        // Parse the due date if the input is provided.
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
        
        // Return a new Task object with the provided input.
        return new Task(0, title, description, priority, isImportant, dueDate);
    }

    public Task GetTaskUpdatesFromUser(Task currentTask)
    {
        
        return currentTask;
    }


    // Prompts the user to input a task ID and validates the input.
    public int GetTaskIdFromUser()
    {
        Console.Write("Enter task ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            return id;
        }
        
        // Show an error message and return -1 for invalid input.
        ShowError("Invalid ID format.");
        return -1;
    }

    // Prompts the user for a yes/no confirmation and returns the result as a boolean.
    public bool GetConfirmationFromUser(string message)
    {
        Console.Write(message);
        string response = Console.ReadLine()?.ToLower() ?? "";
        return response == "y" || response == "yes";
    }

    // Displays a notification message to the user.
    public void ShowNotification(string message)
    {
        Console.WriteLine();
        Console.WriteLine($">>> {message}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // Displays an error message to the user.
    public void ShowError(string message)
    {
        Console.WriteLine();
        Console.WriteLine($"Error: {message}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

// Displays a table of tasks in a formatted layout.
    private void DisplayTaskTable(List<Task> tasks)
    {
        Console.WriteLine();
        Console.WriteLine("ID | Title                  | Priority | Important | Due Date   | Status");
        Console.WriteLine("---+------------------------+----------+-----------+------------+----------");
        
        // Iterate through the list of tasks and display each one in a row.
        foreach (var task in tasks)
        {
            string title = task.Title.Length > 20 ? task.Title.Substring(0, 17) + "..." : task.Title.PadRight(20);
            string priority = task.TaskPriority.ToString().PadRight(8);
            string important = (task.IsImportant ? "Yes" : "No").PadRight(9);
            string dueDate = (task.DueDate.HasValue ? task.DueDate.Value.ToShortDateString() : "-").PadRight(10);
            string status = task.IsCompleted ? "Completed" : "Pending";
            
            // Display the task details in a formatted row.
            Console.WriteLine($"{task.Id.ToString().PadRight(2)} | {title} | {priority} | {important} | {dueDate} | {status}");
        }
    }
}
