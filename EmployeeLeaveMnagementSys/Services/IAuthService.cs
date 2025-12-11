using EmployeeLeaveManagementSys.DTOs;
namespace EmployeeLeaveManagementSys.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterDto registerDto);
        Task<AuthResponse> Login(LoginDto loginDto);
    }
}
