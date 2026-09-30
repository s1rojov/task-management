using Task_management.Data;
using Task_management.Models;

namespace Task_management.Repositories.StatusRepositories
{
    public class StatusRepository : IStatusRepository
    {
        public readonly ApplicationDbContext _context;

        public StatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        //create new status
        public async Task<StatusModel> CreateAsync(StatusModel status)
        {
            await _context.Statuses.AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        //get all status
        public IQueryable<StatusModel> GetAll()
        {
            var statuses = _context.Statuses;
            return statuses;
        }

        //get status by id
        public async Task<StatusModel> GetByIdAsync(int id)
        {
            return await _context.Statuses.FindAsync(id);
        }

        //update status
        public async Task<StatusModel> UpdateAsync(StatusModel status)
        {
            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();
            return status;
        }

        //delete status
        public async Task<StatusModel> DeleteAsync(StatusModel status)
        {
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return status;
        }
    }
}
