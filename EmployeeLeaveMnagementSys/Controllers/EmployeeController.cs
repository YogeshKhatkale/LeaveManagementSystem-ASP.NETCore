using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace EmployeeLeaveManagementSys.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee,Admin")]


    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveService _leaveService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeService employeeService,
            ILeaveService leaveService,
            ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _leaveService = leaveService;
            _logger = logger;
        }

        /// <summary>
        /// Get current employee's leave balance
        /// </summary>
        [HttpGet("leave-balance")]
        public async Task<IActionResult> GetLeaveBalance()
        {
            var employeeId = GetEmployeeIdFromToken();
            var result = await _employeeService.GetLeaveBalance(employeeId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Apply for leave
        /// </summary>
        [HttpPost("apply-leave")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequestDto leaveRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeId = GetEmployeeIdFromToken();
            var result = await _leaveService.ApplyLeave(employeeId, leaveRequestDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all leave requests for current employee
        /// </summary>
        [HttpGet("leave-requests")]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var employeeId = GetEmployeeIdFromToken();
            var result = await _leaveService.GetEmployeeLeaveRequests(employeeId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get employee dashboard data
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var employeeId = GetEmployeeIdFromToken();
            var result = await _employeeService.GetEmployeeDashboard(employeeId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get current employee profile
        /// </summary>
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var employeeId = GetEmployeeIdFromToken();
            var result = await _employeeService.GetEmployeeProfile(employeeId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // Helper method to get employee ID from JWT token
        private int GetEmployeeIdFromToken()
        {
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(employeeIdClaim);
        }
    }
}

