using Task_management.Models;

namespace Task_management.Services.Users
{
    public interface IUserService
    {
        Task<UserModel> AddUserAsync(UserModel user);
        IQueryable<UserModel> GetAllUsers();
        Task<UserModel> GetUserById(int userId);
        Task<UserModel> ModifyUserAsync(UserModel user);
        Task<UserModel> RemoveUserByIdAsync(int userId);
    }
}
