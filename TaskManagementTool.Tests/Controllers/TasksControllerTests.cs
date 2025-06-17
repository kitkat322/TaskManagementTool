using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementTool.Controllers;
using TaskManagementTool.Models;
using TaskManagementTool.Services;
using Xunit;

namespace TaskManagementTool.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskItemService> _mockService;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockService = new Mock<ITaskItemService>();
            _controller = new TasksController(_mockService.Object);
        }

        [Fact]
        public async Task Create_ValidTask_ReturnsCreatedAtAction()
        {
            // ARRANGE
            var task = new TaskItem
            {
                Titel = "Test",
                Beschreibung = "Test Beschreibung",
                Priorität = PrioritätValue.Medium,
                AktuellerStatus = Status.Offen
            };
            task.SetId(1);

            _mockService.Setup(s => s.CreateAsync(It.IsAny<TaskItem>())).ReturnsAsync(task);

            // ACT
            var result = await _controller.Create(task);
            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);

            // ASSERT
            var returnValue = Assert.IsType<TaskItem>(createdAt.Value);
            Assert.Equal(1, returnValue.ID);
            Assert.Equal("Test", returnValue.Titel);
            Assert.Equal("Test Beschreibung", returnValue.Beschreibung);
            Assert.Equal(PrioritätValue.Medium, returnValue.Priorität);
            Assert.Equal(Status.Offen, returnValue.AktuellerStatus);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTasks()
        {
            // ARRANGE
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Titel = "TestTask",
                    Beschreibung = "TestBeschreibung",
                    Priorität = PrioritätValue.High,
                    AktuellerStatus = Status.Offen
                }
            };
            tasks[0].SetId(1);

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(tasks);

            // ACT
            var result = await _controller.GetAll();

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsTask()
        {
            // ARRANGE
            var task = new TaskItem
            {
                Titel = "TestTask",
                Beschreibung = "TestBeschreibung",
                Priorität = PrioritätValue.High,
                AktuellerStatus = Status.Offen
            };
            task.SetId(5);

            _mockService.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(task);

            // ACT
            var result = await _controller.GetById(5);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal(5, returnValue.ID);
            Assert.Equal("TestTask", returnValue.Titel);
            Assert.Equal("TestBeschreibung", returnValue.Beschreibung);
            Assert.Equal(PrioritätValue.High, returnValue.Priorität);
            Assert.Equal(Status.Offen, returnValue.AktuellerStatus);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // ARRANGE
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((TaskItem)null);

            // ACT
            var result = await _controller.GetById(999);

            // ASSERT
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ExistingTask_ReturnsUpdatedTask()
        {
            // ARRANGE
            var existingTask = new TaskItem
            {
                Titel = "Old Title",
                Beschreibung = "Old Beschreibung",
                Priorität = PrioritätValue.Low,
                AktuellerStatus = Status.Offen
            };
            existingTask.SetId(3);

            var updatedTask = new TaskItem
            {
                Titel = "New Title",
                Beschreibung = "New Beschreibung",
                Priorität = PrioritätValue.High,
                AktuellerStatus = Status.Erledigt
            };

            _mockService.Setup(s => s.UpdateAsync(3, updatedTask)).ReturnsAsync(updatedTask);

            // ACT
            var result = await _controller.Update(3, updatedTask);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal("New Title", returnValue.Titel);
            Assert.Equal("New Beschreibung", returnValue.Beschreibung);
            Assert.Equal(PrioritätValue.High, returnValue.Priorität);
            Assert.Equal(Status.Erledigt, returnValue.AktuellerStatus);
        }

        [Fact]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // ARRANGE
            var updatedTask = new TaskItem
            {
                Titel = "NonExistingTask",
                Beschreibung = "NonExistingBeschreibung",
                Priorität = PrioritätValue.Medium,
                AktuellerStatus = Status.InBearbeitung
            };

            _mockService.Setup(s => s.UpdateAsync(999, updatedTask)).ReturnsAsync((TaskItem)null);

            // ACT
            var result = await _controller.Update(999, updatedTask);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingId_RemovesTask()
        {
            // ARRANGE
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // ACT
            var result = await _controller.Delete(1);

            // ASSERT
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // ARRANGE
            _mockService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

            // ACT
            var result = await _controller.Delete(999);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

