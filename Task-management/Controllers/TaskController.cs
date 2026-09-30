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
        public async Task<ActionResult<TaskModel>> PostAsync([FromBody] TaskModel task)
        {
            TaskModel addedTask = await _taskService.AddAsync(task);
            return StatusCode(201, addedTask);
        }

        [HttpGet("GetList")]
        public async Task<ActionResult<List<TaskListDto>>> GetAll()
        {
            List<TaskListDto> tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

    }
}
