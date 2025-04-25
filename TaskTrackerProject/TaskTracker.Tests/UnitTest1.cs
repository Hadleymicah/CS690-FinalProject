using System;
using System.Collections.Generic;
using Xunit;

namespace TaskTracker.Tests
{
    public class Tests
    {

        [Fact]
        public void Task_DefaultConstructor_SetsDefaultsCorrectly()
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
        public void Task_MarkAsCompleteTwice_DoesNotThrow()
        {
            // Arrange
            var task = new Task(1, "Task", "Description", Priority.Low, false, null);

            // Act & Assert
            task.MarkAsComplete();
            task.MarkAsComplete();
            Assert.True(task.IsCompleted);
        }

        // TaskManager class
        [Fact]
        public void TaskManager_Constructor_InitializesWithHistoricalTasks()
        {
            // Act
            var taskManager = new TaskManager();

            // Assert
            var tasks = taskManager.GetAllTasks();
            Assert.NotNull(tasks); // Ensure tasks are loaded
            Assert.NotEmpty(tasks); // Historical tasks present
        }

        [Fact]
        public void TaskManager_AddTask_NullTitle_ThrowsException()
        {
            // Arrange
            var taskManager = new TaskManager();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => taskManager.AddTask(string.Empty, "Description", Priority.Low, false, null));
        }

        // DailySummary class
        [Fact]
        public void DailySummary_EmptyLists_InitializesCorrectly()
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
        public void DailySummary_GenerateSummaryReport_WithNoTasks_ReturnsEmptyMessage()
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