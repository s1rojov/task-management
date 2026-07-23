using Microsoft.AspNetCore.Mvc;
using Task_management.Models;
using Task_management.Services.Statuses;

namespace Task_management.Controllers
{
    public class StatusController : ControllerBase
    {
        private readonly IStatusService statusService;

        public StatusController(IStatusService statusService)
        {
            this.statusService = statusService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<StatusModel>> PostStatusAsync([FromBody] StatusModel status)
        {
            StatusModel addedStatus = await this.statusService.AddStatusAsync(status);
            return StatusCode(201, addedStatus);
        }

        [HttpGet("GetList")]
        public ActionResult<IQueryable<StatusModel>> GetAllStatuses()
        {
            IQueryable<StatusModel> statuses = this.statusService.GetAllStatuses();
            return Ok(statuses);
        }

        [HttpGet("Get/{statusId}")]
        public async Task<ActionResult<StatusModel>> GetStatusByIdAsync(int statusId)
        {
            StatusModel status = await this.statusService.GetStatusById(statusId);
            return Ok(status);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<StatusModel>> PutStatusAsync([FromBody] StatusModel status)
        {
            StatusModel updatedStatus = await this.statusService.ModifyStatusAsync(status);
            return Ok(updatedStatus);
        }

        [HttpDelete("Delete/{statusId}")]
        public async Task<ActionResult<StatusModel>> DeleteUserByIdAsync(int statusId)
        {
            StatusModel deletedStatus = await this.statusService.RemoveStatusByIdAsync(statusId);
            return Ok(deletedStatus
                );
        }
    }
}
