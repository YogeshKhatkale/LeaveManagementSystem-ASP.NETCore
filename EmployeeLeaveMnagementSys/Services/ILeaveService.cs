using EmployeeLeaveManagementSys.Models;
using EmployeeLeaveManagementSys.DTOs;



namespace EmployeeLeaveManagementSys.Services
{
    public interface ILeaveService
    {
        Task<ServiceResponse> ApplyLeave(int employeeId, LeaveRequestDto leaveRequestDto);
        Task<ServiceResponse<List<LeaveRequestResponseDto>>> GetEmployeeLeaveRequests(int employeeId);
        Task<ServiceResponse<List<LeaveRequestResponseDto>>> GetAllPendingLeaves();
        Task<ServiceResponse> ApproveLeave(int leaveRequestId, string adminRemark);
        Task<ServiceResponse> RejectLeave(int leaveRequestId, string adminRemark);
        Task<ServiceResponse<List<LeaveRequestResponseDto>>> GetLeaveHistory();

    }
}
