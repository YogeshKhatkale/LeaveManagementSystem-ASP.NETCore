using EmployeeLeaveManagementSys.DTOs;
namespace EmployeeLeaveManagementSys.DTOs
{
    public class EmployeeDashboardDto
    {
        public LeaveBalanceDto LeaveBalance { get; set; }
        public List<LeaveRequestResponseDto> UpcomingLeaves { get; set; }
        public List<LeaveRequestResponseDto> RecentRequests { get; set; }
        public int PendingRequestsCount { get; set; }
        public int ApprovedRequestsCount { get; set; }
        public int RejectedRequestsCount { get; set; }
    }

    public class AdminDashboardDto
    {
        public int TotalEmployees { get; set; }
        public int PendingRequestsCount { get; set; }
        public int EmployeesOnLeaveToday { get; set; }
        public int TotalApprovedLeaves { get; set; }
        public Dictionary<string, int> LeaveTypeStats { get; set; }
        public List<LeaveRequestResponseDto> PendingRequests { get; set; }
        public List<LeaveRequestResponseDto> RecentApprovals { get; set; }
    }
}
