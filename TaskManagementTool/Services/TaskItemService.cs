using TaskManagementTool.Models;
using TaskManagementTool.Repository;

namespace TaskManagementTool.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _repository;

        public TaskItemService(ITaskItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            await _repository.AddAsync(task);
            return task;
        }

        public async Task<TaskItem?> UpdateAsync(int id, TaskItem task)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return null;

            existing.Titel = task.Titel;
            existing.Beschreibung = task.Beschreibung;
            existing.Priorität = task.Priorität;
            existing.AktuellerStatus = task.AktuellerStatus;

            await _repository.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
