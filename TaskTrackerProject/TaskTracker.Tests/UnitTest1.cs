using System;
using System.Collections.Generic;
using Xunit;

namespace TaskTracker.Tests
{
    public class Tests
    {

        // Task Class Tests
        [Fact]
        public void Task_DefaultConstructor_SetsDefaultsCorrectly() // Ensures the constructor assigns default values properly.
        {
            // Assume valid parameters for the constructor
            var task = new Task(0, "Default Title", "Default Description", Priority.Low, false, null);

            // Assert default-like behavior
            Assert.Equal(0, task.Id);
            Assert.Equal("Default Title", task.Title);
            Assert.Equal("Default Description", task.Description);
            Assert.Equal(Priority.Low, task.TaskPriority);
            Assert.False(task.IsImportant);
            Assert.False(task.IsCompleted);
            Assert.Null(task.DueDate);
        }

        [Fact]
        public void Task_MarkAsCompleteTwice_DoesNotThrow() // Verifies that marking a task as complete multiple times does not cause errors and maintains the correct state.
        {
            // Arrange
            var task = new Task(1, "Task", "Description", Priority.Low, false, null);

            // Act & Assert
            task.MarkAsComplete();
            task.MarkAsComplete();
            Assert.True(task.IsCompleted);
        }

        // TaskManager Class Tests
        [Fact]
        public void TaskManager_Constructor_InitializesWithHistoricalTasks() //Ensures that the TaskManager constructor correctly initializes with provided tasks.
        {
            // Arrange: Provide mock historical tasks
            var mockTasks = new List<Task>
            {
                new Task(1, "Mock Task 1", "Description 1", Priority.Low, false, null),
                new Task(2, "Mock Task 2", "Description 2", Priority.High, true, DateTime.Now.AddDays(1))
            };
            var taskManager = new TaskManager(mockTasks);

            // Act
            var tasks = taskManager.GetAllTasks();

            // Assert
            Assert.NotNull(tasks);
            Assert.NotEmpty(tasks); // Verify that the historical tasks are loaded
            Assert.Equal(2, tasks.Count); // Verify the correct number of tasks
        }

        [Fact]
        public void TaskManager_AddTask_NullTitle_ThrowsException() //Validates that adding a task with a null or empty title throws an appropriate exception.
        {
            // Arrange
            var taskManager = new TaskManager();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => taskManager.AddTask(string.Empty, "Description", Priority.Low, false, null));
        }

        // DailySummary Class Tests
        [Fact]
        public void DailySummary_EmptyLists_InitializesCorrectly() // Confirms that the DailySummary initializes correctly with empty task lists.
        {
            // Arrange
            DateTime today = DateTime.Today;

            // Act
            var summary = new DailySummary(today, new List<Task>(), new List<Task>());

            // Assert
            Assert.Equal(today, summary.Date);
            Assert.Empty(summary.CompletedTasks);
            Assert.Empty(summary.PendingTasks);
        }

        [Fact]
        public void DailySummary_GenerateSummaryReport_WithNoTasks_ReturnsEmptyMessage() //Tests that the summary report correctly reflects the absence of completed and pending tasks.
        {
            // Arrange
            DateTime today = DateTime.Today;
            var summary = new DailySummary(today, new List<Task>(), new List<Task>());

            // Act
            var report = summary.GenerateSummaryReport();

            // Assert
            Assert.Contains("No tasks completed today", report);
            Assert.Contains("No pending tasks", report);
        }
    }
}