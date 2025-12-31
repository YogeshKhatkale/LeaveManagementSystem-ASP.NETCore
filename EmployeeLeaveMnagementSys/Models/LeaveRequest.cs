using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementSys.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public string? LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending";
        public string? AdminRemark { get; set; }
        public DateTime DateSubmitted { get; set; }
        public virtual Employee? Employee { get; set; }

    }
}
