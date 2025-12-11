using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementSys.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }


        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }    
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Role { get; set; } // e.g., "Employee", "Manager", "Admin"

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual LeaveBalance LeaveBalance { get; set; }


    }
}
