using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeLeaveManagementSys.Migrations
{
    /// <inheritdoc />
    public partial class AddReasonToLeaveRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "LeaveRequests");
        }
    }
}
