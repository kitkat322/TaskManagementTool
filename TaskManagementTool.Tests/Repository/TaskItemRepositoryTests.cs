using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementTool.Data;
using TaskManagementTool.Models;
using TaskManagementTool.Repository;

namespace TaskManagementTool.Tests.Repository
{
    public class TaskItemRepositoryTests
    {
        private TaskItemDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskItemDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskItemDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsTask()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskItemRepository(context);

            var task = new TaskItem
            {
                Titel = "Test Task",
                Beschreibung = "Beschreibung",
                Priorität = PrioritätValue.Medium,
                AktuellerStatus = Status.Offen
            };

            // Act
            await repository.AddAsync(task);

            // Assert
            var allTasks = await repository.GetAllAsync();
            Assert.Single(allTasks);
            Assert.Equal("Test Task", allTasks.First().Titel);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectTask()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskItemRepository(context);

            var task = new TaskItem { Titel = "Task 1" };
            await repository.AddAsync(task);

            // Act
            var result = await repository.GetByIdAsync(task.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Task 1", result!.Titel);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesTask()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskItemRepository(context);

            var task = new TaskItem { Titel = "Old", Beschreibung = "Desc" };
            await repository.AddAsync(task);

            // Modify
            task.Titel = "New Title";
            task.Beschreibung = "Updated";

            // Act
            await repository.UpdateAsync(task);
            var result = await repository.GetByIdAsync(task.ID);

            // Assert
            Assert.Equal("New Title", result!.Titel);
            Assert.Equal("Updated", result.Beschreibung);
        }

        [Fact]
        public async Task DeleteAsync_RemovesTask()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskItemRepository(context);

            var task = new TaskItem { Titel = "To be deleted" };
            await repository.AddAsync(task);

            // Act
            await repository.DeleteAsync(task.ID);
            var result = await repository.GetByIdAsync(task.ID);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTasks()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskItemRepository(context);

            await repository.AddAsync(new TaskItem { Titel = "T1" });
            await repository.AddAsync(new TaskItem { Titel = "T2" });

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}