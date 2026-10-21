using Microsoft.AspNetCore.Mvc;
using Task_management.DTOs.Tasks;
using Task_management.Services.Tasks;

namespace Task_management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<TaskListDto>> PostAsync([FromBody] TaskListDto task)
        {
            //{
            //    TaskModel addedTask = await _taskService.AddAsync(task);
            //    return StatusCode(201, addedTask);
            await _taskService.AddAsync(new TaskModel
            {
                Title = task.Title,
                Description = task.Description,
                StatusId = task.StatusId,
                UserId = task.UserId
            }
               );
            return StatusCode(201, "Task addded");
        }


        [HttpGet("GetList")]
        public async Task<ActionResult<List<TaskListDto>>> GetAll()
        {
            List<TaskListDto> tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<TaskListDto>> GetByIdAsync(int id)
        {
            TaskListDto task = await _taskService.GetByIdAsync(id);
            return Ok(task);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<TaskListDto>> PutTaskAsync([FromBody] TaskListDto task)
        {
            TaskListDto updatedTask = await this._taskService.UpdateAsync(task);
            return Ok(updatedTask);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<TaskModel>> DeleteTaskByIdAsync(int id)
        {
            TaskModel deletedTask = await this._taskService.DeleteAsync(id);
            return Ok(deletedTask);
        }

    }
}
