using Task_management.Models;

namespace Task_management.Repositories.StatusRepositories
{
    public interface IStatusRepository
    {
        Task<StatusModel> CreateAsync(StatusModel status);
        IQueryable<StatusModel> GetAll();
        Task<StatusModel> GetByIdAsync(int id);
        Task<StatusModel> UpdateAsync(StatusModel status);
        Task<StatusModel> DeleteAsync(StatusModel status);
    }
}
