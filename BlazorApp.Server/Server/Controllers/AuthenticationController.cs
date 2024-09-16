using BlazorApp.Interfaces;
using BlazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ISession = BlazorApp.Interfaces.ISession;
namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly ISession _sessionService;
        public AuthenticationController(IUser user,ISession session)
        {
            _userService = user;
            _sessionService = session;
        }

        // Register a new user
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
            {
                return BadRequest(new { Message = "Invalid registration data" });
            }

            // Register the user
            var registerUser = await _userService.Register(data);

            if (registerUser == null || !registerUser.Status)
            {
                return BadRequest(new { Message = registerUser?.Message ?? "Registration failed" });
            }

            return Ok(new { Message = "Registration successful" });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel data)
        {


            var userLogin = await _userService.AuthenticateUser(data);

            if (userLogin == null || !userLogin.Status)
            {

                return BadRequest(new { Message = userLogin?.Message ?? "Registration failed" });
            }

            _sessionService.SetUserCookie(data.Credential, true); // Use session service for cookie management
            _sessionService.SetSession("IsUserLoggedIn", "true");


            return Ok();
        }

    }
}
