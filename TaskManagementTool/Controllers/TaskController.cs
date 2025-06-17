using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TaskManagementTool.Data;
using TaskManagementTool.Models;

namespace TaskManagementTool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskItemDbContext _context;

        public TasksController(TaskItemDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            return await _context.Tasks.ToListAsync();
        }


        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = task.ID }, task);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();
            return task;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem task)
        {

            var existing = await _context.Tasks.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Titel = task.Titel;
            existing.Beschreibung = task.Beschreibung;
            existing.Priorität = task.Priorität;
            existing.AktuellerStatus = task.AktuellerStatus;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
