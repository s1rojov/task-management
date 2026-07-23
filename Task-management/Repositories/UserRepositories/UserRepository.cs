using Task_management.Data;
using Task_management.Models;
namespace Task_management.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public IQueryable<UserModel> GetAllUsers()
        {
            var users = _context.Users;
            return users;

        }
        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> DeleteUserAsync(UserModel user)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
