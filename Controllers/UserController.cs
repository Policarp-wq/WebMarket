using Microsoft.AspNetCore.Mvc;
using WebMarket.Contracts;
using WebMarket.DataAccess.Models;
using WebMarket.Services;

namespace WebMarket.Controllers
{
    public class UserController : MyController<User>
    {
        private readonly UserService _userService;
        public UserController(UserService userService) : base(userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationInfo userInfo)
        {
            if (userInfo == null)
                return BadRequest("Body was null");
            var res = await _userService.RegisterUser(userInfo.Login, userInfo.Password, userInfo.Email, userInfo.Address);
            return CreatedAtAction(nameof(RegisterUser), res);
        }
        [HttpGet("{login}")]
        public async Task<IActionResult> GetUserByLogin(string login)
        {
            var user = await _userService.GetByLogin(login);
            if (user == null)
                return new NotFoundObjectResult($"Failed to get user with given login: {login}");
            return new OkObjectResult(user);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoPatch userInfo)
        {
            return new OkObjectResult(await _userService.Update(userInfo.Id, userInfo.Login, userInfo.Email, userInfo.Address));
        }
    }
}
