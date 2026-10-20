using Task_management.DTOs.Tasks;

namespace Task_management.Services.Tasks
{
    public interface ITaskService
    {
        Task<TaskModel> AddAsync(TaskModel task);
        Task<List<TaskListDto>> GetAllAsync();
        Task<TaskListDto> GetByIdAsync(int id);
        Task<TaskListDto> UpdateAsync(TaskListDto task);
        Task<TaskModel> DeleteAsync(int id);
    }
}
