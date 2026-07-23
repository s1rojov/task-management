using Task_management.Models;

namespace Task_management.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUserAsync(UserModel user);
        IQueryable<UserModel> GetAllUsers();
        Task<UserModel> GetUserByIdAsync(int id);
        Task<UserModel> UpdateUserAsync(UserModel user);
        Task<UserModel> DeleteUserAsync(UserModel user);
    }
}
