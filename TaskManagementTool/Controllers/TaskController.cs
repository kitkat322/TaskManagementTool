using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TaskManagementTool.Data;
using TaskManagementTool.Models;
using TaskManagementTool.Services;

namespace TaskManagementTool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskItemService _service;

        public TasksController(ITaskItemService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var tasks = await _service.GetAllAsync();
            return Ok(tasks);
        }


        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(task);

            return CreatedAtAction(nameof(GetById), new { id = task.ID }, task);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem task)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, task);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
