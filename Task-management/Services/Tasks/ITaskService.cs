using Task_management.DTOs.Tasks;

namespace Task_management.Services.Tasks
{
    public interface ITaskService
    {
        Task<TaskModel> AddAsync(TaskModel task);
        Task<List<TaskListDto>> GetAllAsync();
        Task<TaskModel> GetByIdAsync(int id);
        Task<TaskModel> UpdateAsync(TaskModel task);
        Task<TaskModel> DeleteAsync(TaskModel task);
    }
}
