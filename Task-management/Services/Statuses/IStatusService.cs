using Task_management.Models;

namespace Task_management.Services.Statuses
{
    public interface IStatusService
    {
        Task<StatusModel> AddStatusAsync(StatusModel status);
        IQueryable<StatusModel> GetAllStatuses();
        Task<StatusModel> GetStatusById(int statusId);
        Task<StatusModel> ModifyStatusAsync(StatusModel status);
        Task<StatusModel> RemoveStatusByIdAsync(int statusId);
    }
}
