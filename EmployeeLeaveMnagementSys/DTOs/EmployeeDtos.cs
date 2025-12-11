namespace EmployeeLeaveManagementSys.DTOs
{
    public class LeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        public int EmployeeId { get; set; }
        public int AnnualLeave { get; set; }
        public int SickLeave { get; set; }
        public int CasualLeave { get; set; }
        public int OtherLeave { get; set; }
        public int TotalLeaves { get; set; }
    }

    public class EmployeeProfileDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Role { get; set; }
    }
}
