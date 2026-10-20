using Microsoft.EntityFrameworkCore;
using Task_management.DTOs.Tasks;
using Task_management.Repositories.TaskRepositories;
namespace Task_management.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public Task<TaskModel> AddAsync(TaskModel task)
        {
            return _taskRepository.CreateAsync(task);
        }

        public Task<TaskModel> DeleteAsync(int id)
        {
            return _taskRepository.DeleteAsync(id);
        }

        public async Task<List<TaskListDto>> GetAllAsync()
        {
            return await _taskRepository.GetAll()
                .Select(t => new TaskListDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UpdatedAt = t.UpdatedAt,
                    StatusId = t.Status.Id,
                    StatusName = t.Status.Title,
                    UserId = t.User.Id,
                    UserName = t.User.FirstName + " " + t.User.LastName
                })
                .ToListAsync();
        }


        public async Task<TaskListDto> GetByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
                throw new Exception("Task not found");

            return new TaskListDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                UpdatedAt = task.UpdatedAt,
                StatusId = task.Status.Id,
                StatusName = task.Status.Title,
                UserId = task.User.Id,
                UserName = task.User.FirstName + " " + task.User.LastName
            };
        }

        public Task<TaskListDto> UpdateAsync(TaskListDto task)
        {
            return _taskRepository.UpdateAsync(task);
        }



    }
}
