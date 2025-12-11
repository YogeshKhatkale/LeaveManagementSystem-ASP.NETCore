using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeLeaveManagementSys.Services
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthService(ApplicationDbContext context , IConfiguration configuration , ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<AuthResponse> Register(RegisterDto registerDto)
        {
            try
            {
                // check if email already exists
                if (await _context.Employees.AnyAsync(e => e.Email == registerDto.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email already exists!! Use try with another Email. "
                    };
                }

                // create new employee

                var employee = new Employee
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    Department = registerDto.Department,
                    Designation = registerDto.Designation,
                    Role = "Employee"
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                // Create initial leave balance
                var leaveBalance = new LeaveBalance
                {
                    EmployeeId = employee.EmployeeId,
                    AnnualLeave = 20,
                    SickLeave = 10,
                    CasualLeave = 5,
                    OtherLeave = 0
                };

                _context.LeaveBalances.Add(leaveBalance);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Employee registered successfully:{employee.Email}");

                return new AuthResponse
                {
                    Success = true,
                    Message = "Registration successful",
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                    Email = employee.Email,
                    Role = employee.Role
                };
            }
            catch (Exception ex)
            { _logger.LogError($"Registration error:{ex.Message}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Registration failed. Please try again."

                };
            }
        }
        public async Task<AuthResponse> Login(LoginDto loginDto)
        {
            try
            {
                // Find  employee by email
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
                if (employee == null)
                {
                    return new AuthResponse
                    {
                        Success = false, 
                        Message = "Invalid email or password."
                    };
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, employee.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or Password."
                    };
                }
                // Generate JWT token
                var token = GenerateJwtToken(employee);
                _logger.LogInformation($"Employee logged in successfully:{employee.Email}");
                return new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                    Email = employee.Email
                };
            }

            catch (Exception ex)
            {
                _logger.LogError($"Login error:{ex.Message}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Login failed. Please try again."
                };
            }
        }

        private string GenerateJwtToken(Employee employee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(_configuration["JwtSetting:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,employee.EmployeeId.ToString()),
                    new Claim(ClaimTypes.Email,employee.Email),
                    new Claim(ClaimTypes.Name,employee.Name),
                    new Claim(ClaimTypes.Role,employee.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
