using Microsoft.AspNetCore.Mvc;
using Task_management.Models;
using Task_management.Services.Users;

namespace Task_management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserModel>> PostUserAsync([FromBody] UserModel user)
        {
            UserModel addedUser = await this.userService.AddUserAsync(user);
            return StatusCode(201, addedUser);
        }

        [HttpGet("GetList")]
        public ActionResult<IQueryable<UserModel>> GetAllUsers()
        {
            IQueryable<UserModel> users = this.userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("Get/{userId}")]
        public async Task<ActionResult<UserModel>> GetUserByIdAsync(int userId)
        {
            UserModel user = await this.userService.GetUserById(userId);
            return Ok(user);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UserModel>> PutUserAsync([FromBody] UserModel user)
        {
            UserModel updatedUser = await this.userService.ModifyUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("Delete/{userId}")]
        public async Task<ActionResult<UserModel>> DeleteUserByIdAsync(int userId)
        {
            UserModel deletedUser = await this.userService.RemoveUserByIdAsync(userId);
            return Ok(deletedUser);
        }

    }
}
