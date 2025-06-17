using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementTool.Controllers;
using TaskManagementTool.Data;
using TaskManagementTool.Models;

namespace TaskManagementTool.Tests
{
    public class TasksControllerTests
    {
        private TaskItemDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskItemDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            return new TaskItemDbContext(options);
        }

        [Fact]
        public async Task Create_ValidTask_ReturnsCreatedAtAction()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var controller = new TasksController(context);

            var newTask = new TaskItem
            {
                Titel = "Test Task",
                Beschreibung = "Test Beschreibung",
                Priorität = PrioritätValue.Medium,
                AktuellerStatus = Status.Offen
            };

            // ACT
            var result = await controller.Create(newTask);

            // ASSERT
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TaskItem>(createdResult.Value);
            Assert.Equal("Test Task", returnValue.Titel);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTasks()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            context.Tasks.Add(new TaskItem
            {
                Titel = "TestTask",
                Beschreibung = "TestBeschreibung",
                Priorität = PrioritätValue.Low,
                AktuellerStatus = Status.Offen
            });
            context.SaveChanges();

            var controller = new TasksController(context);

            // ACT
            var result = await controller.GetAll();

            // ASSERT
            var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(result.Value);
            Assert.Single(tasks);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsTask()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var task = new TaskItem
            {
                Titel = "Task",
                Beschreibung = "Beschreibung",
                Priorität = PrioritätValue.High,
                AktuellerStatus = Status.Offen
            };
            context.Tasks.Add(task);
            context.SaveChanges();

            var controller = new TasksController(context);

            // ACT
            var result = await controller.GetById(task.ID);

            // ASSERT
            var actionResult = Assert.IsType<ActionResult<TaskItem>>(result);
            var returnValue = Assert.IsType<TaskItem>(actionResult.Value);
            Assert.Equal("Task", returnValue.Titel);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var controller = new TasksController(context);

            // ACT
            var result = await controller.GetById(999);

            // ASSERT
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ExistingId_UpdatesTask()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var task = new TaskItem
            {
                Titel = "Old Title",
                Beschreibung = "Old Beschreibung",
                Priorität = PrioritätValue.Low,
                AktuellerStatus = Status.Offen
            };
            context.Tasks.Add(task);
            context.SaveChanges();

            var controller = new TasksController(context);
            var updatedTask = new TaskItem
            {
                Titel = "New Title",
                Beschreibung = "New Beschreibung",
                Priorität = PrioritätValue.High,
                AktuellerStatus = Status.Erledigt
            };

            // ACT
            var result = await controller.Update(task.ID, updatedTask);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal("New Title", returnValue.Titel);


            var updatedFromDb = context.Tasks.First(t => t.ID == task.ID);
            Assert.Equal("New Title", updatedFromDb.Titel);
        }

        [Fact]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var controller = new TasksController(context);

            var updatedTask = new TaskItem
            {
                Titel = "Task1",
                Beschreibung = "Beschreibung1",
                Priorität = PrioritätValue.Medium,
                AktuellerStatus = Status.InBearbeitung
            };

            // ACT
            var result = await controller.Update(999, updatedTask);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingId_RemovesTask()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var task = new TaskItem
            {
                Titel = "Task will be deleted",
                Beschreibung = "Beschreibung will be deleted",
                Priorität = PrioritätValue.Low,
                AktuellerStatus = Status.Offen
            };
            context.Tasks.Add(task);
            context.SaveChanges();

            var controller = new TasksController(context);

            // ACT
            var result = await controller.Delete(task.ID);

            // ASSERT
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Tasks);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();
            var controller = new TasksController(context);

            // ACT
            var result = await controller.Delete(999);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

