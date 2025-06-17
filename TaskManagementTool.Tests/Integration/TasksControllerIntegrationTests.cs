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
using TaskManagementTool.Repository;
using TaskManagementTool.Services;

namespace TaskManagementTool.Tests.Integration
{
    public class TasksControllerIntegrationTests
    {
        private TasksController GetControllerWithInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<TaskItemDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new TaskItemDbContext(options);
            var repository = new TaskItemRepository(context);
            var service = new TaskItemService(repository);
            var controller = new TasksController(service);

            return controller;
        }

        [Fact]
        public async Task Create_Then_GetById_ReturnsCreatedTask()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb("Create_Then_GetById");
            var newTask = new TaskItem
            {
                Titel = "Integration Test Task",
                Beschreibung = "Beschreibung",
                Priorität = PrioritätValue.High,
                AktuellerStatus = Status.Offen
            };

            // Act – Create
            var createResult = await controller.Create(newTask);
            var createdAt = Assert.IsType<CreatedAtActionResult>(createResult.Result);
            var createdTask = Assert.IsType<TaskItem>(createdAt.Value);

            // Act – Get by ID
            var getResult = await controller.GetById(createdTask.ID);
            var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
            var fetchedTask = Assert.IsType<TaskItem>(okResult.Value);

            // Assert
            Assert.Equal("Integration Test Task", fetchedTask.Titel);
            Assert.Equal(PrioritätValue.High, fetchedTask.Priorität);
        }

        [Fact]
        public async Task Create_Then_Delete_ReturnsNoContent()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb("Create_Then_Delete");
            var task = new TaskItem { Titel = "TaskToDelete" };

            // Act
            var createResult = await controller.Create(task);
            var createdTask = (createResult.Result as CreatedAtActionResult)?.Value as TaskItem;

            Assert.NotNull(createdTask);

            // Act – Delete
            var deleteResult = await controller.Delete(createdTask!.ID);
            Assert.IsType<NoContentResult>(deleteResult);

            // Act – Try get again
            var getResult = await controller.GetById(createdTask.ID);
            Assert.IsType<NotFoundResult>(getResult.Result);
        }
    }
}
