using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EmployeeLeaveManagementSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]

    public class AdminController:ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            ILeaveService leaveService,
            IEmployeeService employeeService,
            ILogger<AdminController> logger)
        {
            _leaveService = leaveService;
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all pending leave requests
        /// </summary>
        [HttpGet("pending-leaves")]
        public async Task<IActionResult> GetPendingLeaves()
        {
            var result = await _leaveService.GetAllPendingLeaves();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Approve a leave request
        /// </summary>
        [HttpPut("approve-leave/{id}")]
        public async Task<IActionResult> ApproveLeave(int id, [FromBody] LeaveApprovalDto approvalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _leaveService.ApproveLeave(id, approvalDto.AdminRemark);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Reject a leave request
        /// </summary>
        [HttpPut("reject-leave/{id}")]
        public async Task<IActionResult> RejectLeave(int id, [FromBody] LeaveApprovalDto approvalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _leaveService.RejectLeave(id, approvalDto.AdminRemark);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get complete leave history of all employees
        /// </summary>
        [HttpGet("leave-history")]
        public async Task<IActionResult> GetLeaveHistory()
        {
            var result = await _leaveService.GetLeaveHistory();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get admin dashboard data
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var result = await _employeeService.GetAdminDashboard();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await _employeeService.GetAllEmployees();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
