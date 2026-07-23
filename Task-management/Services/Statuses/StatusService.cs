
using Task_management.Models;
using Task_management.Repositories.StatusRepositories;

namespace Task_management.Services.Statuses
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _context;
        public StatusService(IStatusRepository context)
        {
            _context = context;
        }

        public Task<StatusModel> AddStatusAsync(StatusModel status)
        {
            return _context.CreateStatusAsync(status);
        }

        public IQueryable<StatusModel> GetAllStatuses()
        {
            return _context.GetAllStatuses();
        }

        public Task<StatusModel> GetStatusById(int statusId)
        {
            return _context.GetByIdAsync(statusId);
        }

        public Task<StatusModel> ModifyStatusAsync(StatusModel status)
        {
            return _context.UpdateStatusAsync(status);
        }

        public async Task<StatusModel> RemoveStatusByIdAsync(int statusId)
        {
            StatusModel status = await _context.GetByIdAsync(statusId);
            return await _context.DeleteStatusAsync(status);
        }


    }
}
