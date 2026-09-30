
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
            return _context.CreateAsync(status);
        }

        public IQueryable<StatusModel> GetAllStatuses()
        {
            return _context.GetAll();
        }

        public Task<StatusModel> GetStatusById(int statusId)
        {
            return _context.GetByIdAsync(statusId);
        }

        public Task<StatusModel> ModifyStatusAsync(StatusModel status)
        {
            return _context.UpdateAsync(status);
        }

        public async Task<StatusModel> RemoveStatusByIdAsync(int statusId)
        {
            StatusModel status = await _context.GetByIdAsync(statusId);
            return await _context.DeleteAsync(status);
        }


    }
}
