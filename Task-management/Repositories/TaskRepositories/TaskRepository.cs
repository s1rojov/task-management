using Task_management.Data;
using Task_management.Models;

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
        async Task<TaskModel> ITaskRepository.DeleteAsync(TaskModel task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }

        //update task
        async Task<TaskModel> ITaskRepository.UpdateAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;

        }

        //get by id

        async Task<TaskModel> ITaskRepository.GetByIdAsync(int id)
        {
            TaskModel task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                throw new Exception("Task not found");
            }
            return task;
        }


    }
}
