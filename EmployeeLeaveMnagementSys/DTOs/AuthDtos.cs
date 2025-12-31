namespace EmployeeLeaveManagementSys.DTOs
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        public string Role { get; set; } = "Employee";


    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }

}
