using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementSys.Models
{
    public class LeaveBalance
    {
        [Key]
        public int  LeaveBalanceId { get; set; }
        public int EmployeeId { get; set; }
        public int AnnualLeave { get; set; } = 20;
        public int SickLeave { get; set; } = 10;
        public int CasualLeave { get; set; } = 5;
        public int OtherLeave { get; set; } = 0;

        public virtual Employee Employee { get; set; }


    }
}
