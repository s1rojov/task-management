using Task_management.DTOs.Tasks;

namespace Task_management.Repositories.TaskRepositories
{
    public interface ITaskRepository
    {
        Task<TaskModel> CreateAsync(TaskModel status);
        IQueryable<TaskModel> GetAll();
        Task<TaskModel> GetByIdAsync(int id);
        Task<TaskListDto> UpdateAsync(TaskListDto task);
        Task<TaskModel> DeleteAsync(int id);
    }
}
