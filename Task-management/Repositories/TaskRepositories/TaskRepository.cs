using Microsoft.EntityFrameworkCore;
using Task_management.Data;
using Task_management.DTOs.Tasks;

namespace Task_management.Repositories.TaskRepositories
{
    public class TaskRepository : ITaskRepository
    {
        public readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //create new task
        async Task<TaskModel> ITaskRepository.CreateAsync(TaskModel task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        //getlist of all tasks
        IQueryable<TaskModel> ITaskRepository.GetAll()
        {
            var tasks = _context.Tasks;
            return tasks;
        }

        //delete task
        async Task<TaskModel> ITaskRepository.DeleteAsync(int id)
        {
            TaskModel task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }

        //update task
        //async Task<TaskListDto> ITaskRepository.UpdateAsync(TaskListDto task)
        //{
        //    var updatedTask = new TaskModel
        //    {

        //    }
        //    _context.Tasks.Update(task);
        //    await _context.SaveChangesAsync();
        //    return task;

        //}


        public async Task<TaskListDto> UpdateAsync(TaskListDto task)
        {
            var existingTask = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == task.Id);

            if (existingTask == null)
                throw new Exception("Task not found");

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.StatusId = task.StatusId;
            existingTask.UserId = task.UserId;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return task;
        }

        //get by id

        async Task<TaskModel> ITaskRepository.GetByIdAsync(int id)
        {
            TaskModel task = await _context.Tasks
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                throw new Exception("Task not found");
            }
            return task;
        }


    }
}
