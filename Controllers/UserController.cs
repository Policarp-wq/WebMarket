using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebMarket.Authorization.JWT;
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

        private void AppendTokenToCookies(string token)
        {
            HttpContext.Response.Cookies.Append(JWTOptions.CookiesName, token);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationInfo userInfo)
        {
            if (userInfo == null)
                return BadRequest("Body was null");
            var authed = await _userService.RegisterUser(userInfo.Login, userInfo.Password, userInfo.Email, userInfo.Address);
            AppendTokenToCookies(authed.Token);
            return CreatedAtAction(nameof(RegisterUser), authed.User);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginRequest)
        {
            try
            {
                var authed = await _userService.Login(loginRequest.Login, loginRequest.Password);
                AppendTokenToCookies(authed.Token);
                return Ok(authed.User);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoPatch userInfo)
        {
            //Extract middleware?
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
            if (!int.TryParse(id, out int userId))
                return BadRequest("Bad token");
            return Ok(await _userService.Update(userInfo.UserId, userInfo.Login, userInfo.Email, userInfo.Address));
        }
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetUserCart()
        {
            var id = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
            if (!int.TryParse(id, out int userId))
                return BadRequest("Bad token");
            var elements = await _userService.GetUserShoppingCart(userId);
            return Ok(elements);
        }
    }
}
