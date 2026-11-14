using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.ViewModel.Dto.LoginDto;
using SMT.ViewModel.Dto.UserDto;
using System.Threading.Tasks;
using SMT.Security;

namespace SMT.Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreate userCreate)
        {
            var result = await _userService.Create(userCreate, "user");

            return Ok(result);
        }
        
        // Only an admin may create another admin. The very first admin can still be created
        // anonymously, otherwise a fresh database could never get one.
        [HttpPost]
        [Route("/admin/register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdmin([FromBody] UserCreate userCreate)
        {
            if (!User.IsInRole("admin") && await _userService.AnyUserInRoleAsync("admin"))
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Only an admin can create admin users" });

            var result = await _userService.Create(userCreate, "admin");

            return Ok(result);
        }

        [HttpPost]
        [Route("auth")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody]  UserLogin user)
        {
            var result = await _userService.Authenticate(user.Username, user.Password);

            return Ok(result);
        }

        [HttpGet("GetByUsername")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var result = await _userService.GetUserByUsername(username);

            return Ok(result);
        }
    }
}
