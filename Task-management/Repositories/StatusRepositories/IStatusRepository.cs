using Task_management.Models;

namespace Task_management.Repositories.StatusRepositories
{
    public interface IStatusRepository
    {
        public Task<StatusModel> CreateStatusAsync(StatusModel status);
        public IQueryable<StatusModel> GetAllStatuses();
        public Task<StatusModel> GetByIdAsync(int id);
        public Task<StatusModel> UpdateStatusAsync(StatusModel status);
        public Task<StatusModel> DeleteStatusAsync(StatusModel status);
    }
}
