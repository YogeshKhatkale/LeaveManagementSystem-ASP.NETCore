using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ApplicationDbContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResponse<LeaveBalanceDto>> GetLeaveBalance(int employeeId)
        {
            try
            {
                var leaveBalance = await _context.LeaveBalances
                    .Where(lb => lb.EmployeeId == employeeId)
                    .Select(lb => new LeaveBalanceDto
                    {
                        LeaveBalanceId = lb.LeaveBalanceId,
                        EmployeeId = lb.EmployeeId,
                        AnnualLeave = lb.AnnualLeave,
                        SickLeave = lb.SickLeave,
                        CasualLeave = lb.CasualLeave,
                        OtherLeave = lb.OtherLeave
                    })
                    .FirstOrDefaultAsync();

                if (leaveBalance == null)
                {
                    return new ServiceResponse<LeaveBalanceDto>
                    {
                        Success = false,
                        Message = "Leave balance not found"
                    };
                }

                return new ServiceResponse<LeaveBalanceDto>
                {
                    Success = true,
                    Message = "Leave balance retrieved successfully",
                    Data = leaveBalance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get leave balance error: {ex.Message}");
                return new ServiceResponse<LeaveBalanceDto>
                {
                    Success = false,
                    Message = "Failed to retrieve leave balance"
                };
            }
        }

        public async Task<ServiceResponse<EmployeeDashboardDto>> GetEmployeeDashboard(int employeeId)
        {
            try
            {
                // Get leave balance
                var leaveBalance = await _context.LeaveBalances
                    .Where(lb => lb.EmployeeId == employeeId)
                    .Select(lb => new LeaveBalanceDto
                    {
                        LeaveBalanceId = lb.LeaveBalanceId,
                        EmployeeId = lb.EmployeeId,
                        AnnualLeave = lb.AnnualLeave,
                        SickLeave = lb.SickLeave,
                        CasualLeave = lb.CasualLeave,
                        OtherLeave = lb.OtherLeave
                    })
                    .FirstOrDefaultAsync();

                // Get upcoming leaves
                var upcomingLeaves = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => lr.EmployeeId == employeeId &&
                                lr.Status == "Approved" &&
                                lr.StartDate >= DateTime.Today)
                    .OrderBy(lr => lr.StartDate)
                    .Take(5)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1
                    })
                    .ToListAsync();

                // Get recent requests
                var recentRequests = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => lr.EmployeeId == employeeId)
                    .OrderByDescending(lr => lr.DateSubmitted)
                    .Take(5)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1
                    })
                    .ToListAsync();

                // Get request counts
                var pendingCount = await _context.LeaveRequests
                    .CountAsync(lr => lr.EmployeeId == employeeId && lr.Status == "Pending");

                var approvedCount = await _context.LeaveRequests
                    .CountAsync(lr => lr.EmployeeId == employeeId && lr.Status == "Approved");

                var rejectedCount = await _context.LeaveRequests
                    .CountAsync(lr => lr.EmployeeId == employeeId && lr.Status == "Rejected");

                var dashboard = new EmployeeDashboardDto
                {
                    LeaveBalance = leaveBalance,
                    UpcomingLeaves = upcomingLeaves,
                    RecentRequests = recentRequests,
                    PendingRequestsCount = pendingCount,
                    ApprovedRequestsCount = approvedCount,
                    RejectedRequestsCount = rejectedCount
                };

                return new ServiceResponse<EmployeeDashboardDto>
                {
                    Success = true,
                    Message = "Dashboard data retrieved successfully",
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get employee dashboard error: {ex.Message}");
                return new ServiceResponse<EmployeeDashboardDto>
                {
                    Success = false,
                    Message = "Failed to retrieve dashboard data"
                };
            }
        }

        public async Task<ServiceResponse<AdminDashboardDto>> GetAdminDashboard()
        {
            try
            {
                // Total employees
                var totalEmployees = await _context.Employees.CountAsync();

                // Pending requests count
                var pendingCount = await _context.LeaveRequests
                    .CountAsync(lr => lr.Status == "Pending");

                // Employees on leave today
                var today = DateTime.Today;
                var employeesOnLeave = await _context.LeaveRequests
                    .CountAsync(lr => lr.Status == "Approved" &&
                                     lr.StartDate <= today &&
                                     lr.EndDate >= today);

                // Total approved leaves
                var totalApproved = await _context.LeaveRequests
                    .CountAsync(lr => lr.Status == "Approved");

                // Leave type statistics
                var leaveTypeStats = await _context.LeaveRequests
                    .Where(lr => lr.Status == "Approved")
                    .GroupBy(lr => lr.LeaveType)
                    .Select(g => new { LeaveType = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.LeaveType, x => x.Count);

                // Pending requests
                var pendingRequests = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => lr.Status == "Pending")
                    .OrderBy(lr => lr.DateSubmitted)
                    .Take(10)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1
                    })
                    .ToListAsync();

                // Recent approvals
                var recentApprovals = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => lr.Status == "Approved")
                    .OrderByDescending(lr => lr.DateSubmitted)
                    .Take(10)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1
                    })
                    .ToListAsync();

                var dashboard = new AdminDashboardDto
                {
                    TotalEmployees = totalEmployees,
                    PendingRequestsCount = pendingCount,
                    EmployeesOnLeaveToday = employeesOnLeave,
                    TotalApprovedLeaves = totalApproved,
                    LeaveTypeStats = leaveTypeStats,
                    PendingRequests = pendingRequests,
                    RecentApprovals = recentApprovals
                };

                return new ServiceResponse<AdminDashboardDto>
                {
                    Success = true,
                    Message = "Admin dashboard data retrieved successfully",
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get admin dashboard error: {ex.Message}");
                return new ServiceResponse<AdminDashboardDto>
                {
                    Success = false,
                    Message = "Failed to retrieve admin dashboard data"
                };
            }
        }

        public async Task<ServiceResponse<EmployeeProfileDto>> GetEmployeeProfile(int employeeId)
        {
            try
            {
                var employee = await _context.Employees
                    .Where(e => e.EmployeeId == employeeId)
                    .Select(e => new EmployeeProfileDto
                    {
                        EmployeeId = e.EmployeeId,
                        Name = e.Name,
                        Email = e.Email,
                        Department = e.Department,
                        Designation = e.Designation,
                        Role = e.Role
                    })
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return new ServiceResponse<EmployeeProfileDto>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                return new ServiceResponse<EmployeeProfileDto>
                {
                    Success = true,
                    Message = "Employee profile retrieved successfully",
                    Data = employee
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get employee profile error: {ex.Message}");
                return new ServiceResponse<EmployeeProfileDto>
                {
                    Success = false,
                    Message = "Failed to retrieve employee profile"
                };
            }
        }

        public async Task<ServiceResponse<List<EmployeeProfileDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _context.Employees
                    .Select(e => new EmployeeProfileDto
                    {
                        EmployeeId = e.EmployeeId,
                        Name = e.Name,
                        Email = e.Email,
                        Department = e.Department,
                        Designation = e.Designation,
                        Role = e.Role
                    })
                    .ToListAsync();

                return new ServiceResponse<List<EmployeeProfileDto>>
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = employees
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get all employees error: {ex.Message}");
                return new ServiceResponse<List<EmployeeProfileDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve employees"
                };
            }
        }
    }
}
