using EmployeeLeaveManagementSys.DTOs;

namespace EmployeeLeaveManagementSys.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<LeaveBalanceDto>> GetLeaveBalance(int employeeId);
        Task<ServiceResponse<EmployeeDashboardDto>> GetEmployeeDashboard(int employeeId);
        Task<ServiceResponse<AdminDashboardDto>> GetAdminDashboard();
        Task<ServiceResponse<EmployeeProfileDto>> GetEmployeeProfile(int employeeId);
        Task<ServiceResponse<List<EmployeeProfileDto>>> GetAllEmployees();
    }
}
