using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace TaskTracker.Tests;
using TaskTracker;
public class TaskTrackerTests
{
    // Task class tests
    [Fact]
    public void Task_Construction_SetsPropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        string title = "Test Task";
        string description = "Test Description";
        Priority priority = Priority.High;
        bool isImportant = true;
        DateTime? dueDate = DateTime.Now.AddDays(1);

        // Act
        var task = new Task(id, title, description, priority, isImportant, dueDate);

        // Assert
        Assert.Equal(id, task.Id);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(priority, task.TaskPriority);
        Assert.Equal(isImportant, task.IsImportant);
        Assert.Equal(dueDate, task.DueDate);
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void Task_MarkAsComplete_SetsIsCompletedToTrue()
    {
        // Arrange
        var task = new Task(1, "Test Task", "Description", Priority.Medium, false, null);
        
        // Act
        task.MarkAsComplete();
        
        // Assert
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void Task_UpdatePriority_ChangesPriorityCorrectly()
    {
        // Arrange
        var task = new Task(1, "Test Task", "Description", Priority.Low, false, null);
        
        // Act
        task.UpdatePriority(Priority.High);
        
        // Assert
        Assert.Equal(Priority.High, task.TaskPriority);
    }

    [Fact]
    public void Task_ToggleImportance_ChangesImportanceStatus()
    {
        // Arrange
        var task = new Task(1, "Test Task", "Description", Priority.Medium, false, null);
        
        // Act
        task.ToggleImportance();
        
        // Assert
        Assert.True(task.IsImportant);
        
        // Act again to toggle back
        task.ToggleImportance();
        
        // Assert
        Assert.False(task.IsImportant);
    }

    // TaskManager class tests
    [Fact]
    public void TaskManager_AddTask_AddsTaskCorrectly()
    {
        // Arrange
        var taskManager = new TaskManager();
        string title = "Test Task";
        string description = "Test Description";
        Priority priority = Priority.Medium;
        bool isImportant = false;
        DateTime? dueDate = null;
        
        // Act
        var task = taskManager.AddTask(title, description, priority, isImportant, dueDate);
        var allTasks = taskManager.GetAllTasks();
        
        // Assert
        Assert.Single(allTasks);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(priority, task.TaskPriority);
        Assert.Equal(isImportant, task.IsImportant);
        Assert.Equal(dueDate, task.DueDate);
    }

    [Fact]
    public void TaskManager_RemoveTask_RemovesTaskCorrectly()
    {
        // Arrange
        var taskManager = new TaskManager();
        var task = taskManager.AddTask("Test Task", "Description", Priority.Medium, false, null);
        
        // Act
        bool result = taskManager.RemoveTask(task.Id);
        var allTasks = taskManager.GetAllTasks();
        
        // Assert
        Assert.True(result);
        Assert.Empty(allTasks);
    }

    [Fact]
    public void TaskManager_GetTaskById_ReturnsCorrectTask()
    {
        // Arrange
        var taskManager = new TaskManager();
        var task1 = taskManager.AddTask("Task 1", "Description 1", Priority.Low, false, null);
        var task2 = taskManager.AddTask("Task 2", "Description 2", Priority.High, true, DateTime.Now.AddDays(1));
        
        // Act
        var retrievedTask = taskManager.GetTaskById(task2.Id);
        
        // Assert
        Assert.Equal(task2.Id, retrievedTask.Id);
        Assert.Equal("Task 2", retrievedTask.Title);
    }

    [Fact]
    public void TaskManager_GetTasksByPriority_ReturnsCorrectTasks()
    {
        // Arrange
        var taskManager = new TaskManager();
        taskManager.AddTask("Task 1", "Description 1", Priority.Low, false, null);
        taskManager.AddTask("Task 2", "Description 2", Priority.High, true, null);
        taskManager.AddTask("Task 3", "Description 3", Priority.High, false, null);
        
        // Act
        var highPriorityTasks = taskManager.GetTasksByPriority(Priority.High);
        
        // Assert
        Assert.Equal(2, highPriorityTasks.Count);
        Assert.All(highPriorityTasks, task => Assert.Equal(Priority.High, task.TaskPriority));
    }

    [Fact]
    public void TaskManager_GetCompletedTasks_ReturnsOnlyCompletedTasks()
    {
        // Arrange
        var taskManager = new TaskManager();
        var task1 = taskManager.AddTask("Task 1", "Description 1", Priority.Low, false, null);
        var task2 = taskManager.AddTask("Task 2", "Description 2", Priority.High, true, null);
        
        // Mark task1 as complete
        task1.MarkAsComplete();
        taskManager.UpdateTask(task1);
        
        // Act
        var completedTasks = taskManager.GetCompletedTasks();
        
        // Assert
        Assert.Single(completedTasks);
        Assert.Equal(task1.Id, completedTasks[0].Id);
        Assert.True(completedTasks[0].IsCompleted);
    }

    [Fact]
    public void TaskManager_GetPendingTasks_ReturnsOnlyPendingTasks()
    {
        // Arrange
        var taskManager = new TaskManager();
        var task1 = taskManager.AddTask("Task 1", "Description 1", Priority.Low, false, null);
        var task2 = taskManager.AddTask("Task 2", "Description 2", Priority.High, true, null);
        
        // Mark task1 as complete
        task1.MarkAsComplete();
        taskManager.UpdateTask(task1);
        
        // Act
        var pendingTasks = taskManager.GetPendingTasks();
        
        // Assert
        Assert.Single(pendingTasks);
        Assert.Equal(task2.Id, pendingTasks[0].Id);
        Assert.False(pendingTasks[0].IsCompleted);
    }

    [Fact]
    public void TaskManager_GetImportantTasks_ReturnsOnlyImportantTasks()
    {
        // Arrange
        var taskManager = new TaskManager();
        var task1 = taskManager.AddTask("Task 1", "Description 1", Priority.Low, false, null);
        var task2 = taskManager.AddTask("Task 2", "Description 2", Priority.High, true, null);
        
        // Act
        var importantTasks = taskManager.GetImportantTasks();
        
        // Assert
        Assert.Single(importantTasks);
        Assert.Equal(task2.Id, importantTasks[0].Id);
        Assert.True(importantTasks[0].IsImportant);
    }

    [Fact]
    public void TaskManager_UpdateTask_UpdatesTaskCorrectly()
    {
        // Arrange
        var taskManager = new TaskManager();
        var task = taskManager.AddTask("Original Title", "Original Description", Priority.Low, false, null);
        
        // Modify the task
        task.UpdateTitle("Updated Title");
        task.UpdateDescription("Updated Description");
        task.UpdatePriority(Priority.High);
        task.ToggleImportance();
        task.MarkAsComplete();
        
        // Act
        bool result = taskManager.UpdateTask(task);
        var updatedTask = taskManager.GetTaskById(task.Id);
        
        // Assert
        Assert.True(result);
        Assert.Equal("Updated Title", updatedTask.Title);
        Assert.Equal("Updated Description", updatedTask.Description);
        Assert.Equal(Priority.High, updatedTask.TaskPriority);
        Assert.True(updatedTask.IsImportant);
        Assert.True(updatedTask.IsCompleted);
    }

    // DailySummary class tests
    [Fact]
    public void DailySummary_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        DateTime today = DateTime.Now;
        var completedTasks = new List<Task>
        {
            new Task(1, "Completed Task", "Description", Priority.High, true, null) { IsCompleted = true }
        };
        var pendingTasks = new List<Task>
        {
            new Task(2, "Pending Task", "Description", Priority.Medium, false, today.AddDays(1))
        };
        
        // Act
        var summary = new DailySummary(today, completedTasks, pendingTasks);
        
        // Assert
        Assert.Equal(today, summary.Date);
        Assert.Equal(1, summary.CompletedTasksCount);
        Assert.Equal(1, summary.PendingTasksCount);
        Assert.Equal(completedTasks, summary.CompletedTasks);
        Assert.Equal(pendingTasks, summary.PendingTasks);
    }

    [Fact]
    public void DailySummary_GenerateSummaryReport_ReturnsCorrectReport()
    {
        // Arrange
        DateTime today = DateTime.Now;
        var completedTasks = new List<Task>
        {
            new Task(1, "Completed Task", "Description", Priority.High, true, null) { IsCompleted = true }
        };
        var pendingTasks = new List<Task>
        {
            new Task(2, "Pending Task", "Description", Priority.Medium, false, today.AddDays(1))
        };
        var summary = new DailySummary(today, completedTasks, pendingTasks);
        
        // Act
        string report = summary.GenerateSummaryReport();
        
        // Assert
        Assert.Contains($"Daily Summary for {today.ToShortDateString()}", report);
        Assert.Contains("Tasks completed today: 1", report);
        Assert.Contains("Tasks pending: 1", report);
        Assert.Contains("Completed Task", report);
        Assert.Contains("Pending Task", report);
    }

    [Fact]
    public void DailySummary_GenerateSummaryReport_HandlesUpcomingTasks()
    {
        // Arrange
        DateTime today = DateTime.Now;
        var completedTasks = new List<Task>();
        var pendingTasks = new List<Task>
        {
            new Task(1, "Tomorrow Task", "Description", Priority.High, true, today.AddDays(1)),
            new Task(2, "Later Task", "Description", Priority.Medium, false, today.AddDays(5))
        };
        var summary = new DailySummary(today, completedTasks, pendingTasks);
        
        // Act
        string report = summary.GenerateSummaryReport();
        
        // Assert
        Assert.Contains("Tomorrow Task", report);
        Assert.DoesNotContain("Later Task", report); // Should not appear in upcoming (next 3 days) tasks
    }

    [Fact]
    public void DailySummary_GenerateSummaryReport_HandlesEmptyTaskLists()
    {
        // Arrange
        DateTime today = DateTime.Now;
        var completedTasks = new List<Task>();
        var pendingTasks = new List<Task>();
        var summary = new DailySummary(today, completedTasks, pendingTasks);
        
        // Act
        string report = summary.GenerateSummaryReport();
        
        // Assert
        Assert.Contains("No tasks completed today", report);
        Assert.Contains("No pending tasks", report);
    }
}

