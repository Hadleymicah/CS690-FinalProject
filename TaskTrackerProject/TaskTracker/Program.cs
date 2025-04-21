namespace TaskTracker;

public class Program
{
    private TaskManager taskManager = null!;
    private ConsoleUI? ui;

    public void Initialize()
    {
        taskManager = new TaskManager();
        ui = new ConsoleUI(taskManager);
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            ui.DisplayMainMenu();
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1": // Add New Task
                    ui.DisplayAddTaskScreen();
                    break;
                    
                case "2": // View Dashboard
                    ui.DisplayDashboard();
                    break;
                    
                case "3": // Edit Task
                    int taskId = ui.GetTaskIdFromUser();
                    if (taskId != -1)
                    {
                        var task = taskManager.GetTaskById(taskId);
                        if (task != null)
                        {
                            ui.DisplayEditTaskScreen(task);
                        }
                        else
                        {
                            ui.ShowError("Task not found.");
                        }
                    }
                    break;
                    
                case "4": // Delete Task
                    int deleteTaskId = ui.GetTaskIdFromUser();
                    if (deleteTaskId != -1)
                    {
                        var taskToDelete = taskManager.GetTaskById(deleteTaskId);
                        if (taskToDelete != null)
                        {
                            ui.DisplayDeleteTaskConfirmation(taskToDelete);
                        }
                        else
                        {
                            ui.ShowError("Task not found.");
                        }
                    }
                    break;
                    
                case "5": // Daily Summary
                    var completedTasks = taskManager.GetCompletedTasks();
                    var pendingTasks = taskManager.GetPendingTasks();
                    var summary = new DailySummary(DateTime.Now, completedTasks, pendingTasks);
                    ui.DisplayDailySummary(summary);
                    break;
                    
                case "6": // Exit Application
                    Console.Clear();
                    Console.WriteLine("===================================");
                    Console.WriteLine("Thank you for using TaskTracker. Your tasks have been saved.");
                    Console.WriteLine("Have a productive day!");
                    Console.WriteLine("===================================");
                    running = false;
                    break;
                    
                default:
                    ui.ShowError("Invalid choice. Please enter a number between 1 and 6.");
                    break;
            }
        }
    }

    static void Main(string[] args)
    {
        Program program = new Program();
        program.Initialize();
        program.Run();
    }
}
