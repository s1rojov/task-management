using Task_management.Models;

namespace Task_management.Repositories.TaskRepositories
{
    public interface ITaskRepository
    {
        Task<TaskModel> CreateAsync(TaskModel status);
        IQueryable<TaskModel> GetAll();
        Task<TaskModel?> GetByIdAsync(int id);
        Task<TaskModel> UpdateAsync(TaskModel status);
        Task<TaskModel> DeleteAsync(TaskModel status);
    }
}
