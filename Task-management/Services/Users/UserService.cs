using Task_management.Models;
using Task_management.Repositories.UserRepositories;


namespace Task_management.Services.Users
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserModel> AddUserAsync(UserModel user)
        {
            return _userRepository.CreateUserAsync(user);
        }

        public IQueryable<UserModel> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public Task<UserModel> GetUserById(int userId)
        {
            return _userRepository.GetUserByIdAsync(userId);
        }

        public Task<UserModel> ModifyUserAsync(UserModel user)
        {
            return _userRepository.UpdateUserAsync(user);
        }

        public async Task<UserModel> RemoveUserByIdAsync(int userId)
        {
            UserModel user = await _userRepository.GetUserByIdAsync(userId);
            return await _userRepository.DeleteUserAsync(user);
        }
    }
}
