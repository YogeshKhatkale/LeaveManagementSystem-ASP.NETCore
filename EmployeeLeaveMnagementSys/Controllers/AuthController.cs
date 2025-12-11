using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagementSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        /// <summary>
        /// Register a new employee
        /// </summary>

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(registerDto);

            if (!result.Success)
            {
                return BadRequest(result);

            }
            return Ok(result);
        }



        /// <summary>
        /// Login and get JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}