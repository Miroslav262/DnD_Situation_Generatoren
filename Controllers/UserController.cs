using dndsitgen.Models;
using dndsitgen.Repository;
using dndsitgen.Serveces;
using Microsoft.AspNetCore.Mvc;

namespace dndsitgen.Controllers
{
    public class LoginRequest {
        public string name { get; set; }
        public string pass_hash { get; set; }
    }
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserRepository userRepository;
        private readonly JwtService jwtService;
        public UserController(UserRepository userRepository, JwtService jwtService) {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
        }

        [HttpPost("/user")]
        public async Task<IActionResult> Create([FromBody]UserModel request)
        {
            if (string.IsNullOrEmpty(request.name) || string.IsNullOrEmpty(request.pass_hash))
            {
                return BadRequest("Required name and password");
            }
            else {
                UserModel? user = await userRepository.createUser(request);
                if (user == null) {
                    return Conflict("User alredy exists");
                }
                return Ok(new {
                id = user.id,
                name = user.name,
                });
            }
        }
        [HttpPost("/user/login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request) {
            bool? isValid = await userRepository.checkUser(new UserModel
            {
                name = request.name,
                pass_hash = request.pass_hash
            });

            if (isValid == null || isValid == false) {
                return Unauthorized("Invalid username or password");
            }
            string token = jwtService.Generate(request.name);
            return Ok(new { token });
        }
    }
}
