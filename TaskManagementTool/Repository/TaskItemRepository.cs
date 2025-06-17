using Microsoft.EntityFrameworkCore;
using TaskManagementTool.Data;
using TaskManagementTool.Models;

namespace TaskManagementTool.Repository
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskItemDbContext _context;

        public TaskItemRepository(TaskItemDbContext context)
        {
            _context = context;
        }

        // get all tasks
        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        // get task by ID
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        // add new task
        public async Task AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        // update excisting task
        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        // delete task by ID
        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
