using EmployeeLeaveManagementSys.Models;
namespace EmployeeLeaveManagementSys.DTOs
{
    public class LeaveRequestDto
    {
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }

    public class LeaveApprovalDto 
    {
        public string AdminRemark { get; set; }

    }
    public class LeaveRequestResponseDto
    {
      public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate   { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string AdminRemark { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int TotalDays { get; set; }
    }


}
