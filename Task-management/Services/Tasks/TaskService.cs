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

        public Task<TaskModel> DeleteAsync(TaskModel task)
        {
            return _taskRepository.DeleteAsync(task);
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
                    //Status = new TaskStatusDto
                    //{
                    //    Id = t.Status.Id,
                    //    Name = t.Status.Title
                    //},
                    //User = new TaskUserDto
                    //{
                    //    Id = t.User.Id,
                    //    FullName = t.User.FirstName + " " + t.User.LastName
                    //}
                })
                .ToListAsync();
        }


        public Task<TaskModel> GetByIdAsync(int id)
        {
            return _taskRepository.GetByIdAsync(id);
        }

        public Task<TaskModel> UpdateAsync(TaskModel task)
        {
            return _taskRepository.UpdateAsync(task);
        }
    }
}
