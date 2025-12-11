using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeLeaveManagementSys.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeaveService> _logger;


        public LeaveService(ApplicationDbContext context, ILogger<LeaveService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResponse> ApplyLeave(int employeeId, LeaveRequestDto leaveRequestDto)
        {
            try
            {
                // Validate dates
                if (leaveRequestDto.StartDate < DateTime.Today)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Start date cannot be in the past."
                    };
                }

                if (leaveRequestDto.EndDate < leaveRequestDto.StartDate)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "End date must be after or equal to start date"
                    };
                }
                // Check for Overlapping leaves

                var hasOverlap = await _context.LeaveRequests.AnyAsync(
                    lr => lr.EmployeeId == employeeId && lr.Status == "Approved" &&
                    ((leaveRequestDto.StartDate >= lr.StartDate && leaveRequestDto.StartDate <= lr.EndDate) ||
                    (leaveRequestDto.EndDate >= lr.StartDate && leaveRequestDto.EndDate <= lr.EndDate) ||
                    (leaveRequestDto.StartDate <= lr.StartDate && leaveRequestDto.EndDate >= lr.EndDate)));

                if (hasOverlap)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave dates overlap with existing approved leave"
                    };
                }

                // Calculate days

                var days = (leaveRequestDto.EndDate - leaveRequestDto.StartDate).Days + 1;

                // Check leave balance 
                var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId);

                if (leaveBalance == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave balance not found"
                    };
                }

                int availableDays = leaveRequestDto.LeaveType switch
                {
                    "Annual" => leaveBalance.AnnualLeave,
                    "Sick" => leaveBalance.SickLeave,
                    "Casual" => leaveBalance.CasualLeave,
                    "Other" => leaveBalance.OtherLeave,
                    _ => 0
                };

                if (availableDays < days)
                {

                    return new ServiceResponse
                    {
                        Success = false,
                        Message = $"Insufficient{leaveRequestDto.LeaveType} leave Balance: {availableDays} days, Required: {days} days."

                    };
                }

                // Create leave request
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = employeeId,
                    LeaveType = leaveRequestDto.LeaveType,
                    StartDate = leaveRequestDto.StartDate,
                    EndDate = leaveRequestDto.EndDate,
                    Reason = leaveRequestDto.Reason,
                    Status = "Pending",
                    DateSubmitted = DateTime.UtcNow
                };

                _context.LeaveRequests.Add(leaveRequest);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Leave request submitted by employee {employeeId}");
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Leave request submitted successfully.",
                    Data = leaveRequest.LeaveRequestId

                };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Apply leave error: {ex:Message}");
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Failed to submit leave request. Please try again."
                };
            }


        }
        public async Task<ServiceResponse<List<LeaveRequestResponseDto>>> GetEmployeeLeaveRequests(int employeeId)
        {
            try
            {
                var leaveRequests = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => employeeId == employeeId)
                    .OrderByDescending(lr => lr.DateSubmitted)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1

                    })
                    .ToListAsync();
                return new ServiceResponse<List<LeaveRequestResponseDto>>
                {
                    Success = true,
                    Message = "Leave requests Retrived successfully.",
                    Data = leaveRequests
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get employee leave requests error: {ex:Message}");
                return new ServiceResponse<List<LeaveRequestResponseDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve leave requests"
                };
            }
        }
        public async Task<ServiceResponse<List<LeaveRequestResponseDto>>> GetAllPendingLeaves()
        {
            try
            {
                var pendingLeaves = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => lr.Status == "Pending")
                    .OrderBy(lr => lr.DateSubmitted)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1

                    })
                    .ToListAsync();
                return new ServiceResponse<List<LeaveRequestResponseDto>>
                {
                    Success = true,
                    Message = "Pending leaves retrieved successfully",
                    Data = pendingLeaves
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get pending leaves error: {ex:Message}");
                return new ServiceResponse<List<LeaveRequestResponseDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve pending leaves"
                };
            }
        }

        public async Task<ServiceResponse> ApproveLeave(int leaveRequestId, string adminRemark)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequests
                    .FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
                if (leaveRequest == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave request not found"
                    };
                }
                if (leaveRequest.Status != "Pending")
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave request has already been processed"
                    };
                }

                // calculate days and update leave balance 
                var days = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;
                var leaveBalance = await _context.LeaveBalances
                    .FirstOrDefaultAsync(lb => lb.EmployeeId == leaveRequest.EmployeeId);

                if (leaveBalance == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave balance not found"
                    };
                }

                // Deduct from leave balance 
                switch (leaveRequest.LeaveType)
                {
                    case "Annual":
                        leaveBalance.AnnualLeave -= days;
                        break;
                    case "Sick":
                        leaveBalance.SickLeave -= days;
                        break;
                    case "Casual":
                        leaveBalance.CasualLeave -= days;
                        break;
                    case "Other":
                        leaveBalance.OtherLeave -= days;
                        break;
                }

                leaveRequest.Status = "Approved";
                leaveRequest.AdminRemark = adminRemark;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Leave request {leaveRequestId} approved");
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Leave approved successfully"
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Approve leave error : {ex.Message}");
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Failed to approve leave"
                };
            }
        }
        public async Task<ServiceResponse> RejectLeave(int leaveRequestId, string adminRemark)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequests
                    .FirstOrDefaultAsync(lr => lr.LeaveRequestId == leaveRequestId);
                if (leaveRequest == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave request not found "
                    };
                }
                if (leaveRequest.Status != "Pending")
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Leave request has already been processed"
                    };
                }
                leaveRequest.Status = "Rejected";
                leaveRequest.AdminRemark = adminRemark;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Leave Request {leaveRequestId} rejected");
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Leave rejected successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Reject leave error: {ex.Message}");
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Failed to reject leave"
                };
            }
        }

        public async Task<ServiceResponse<List<LeaveRequestResponseDto>>> GetLeaveHistory()
        {
            try
            {
                var leaveHistory = await _context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .OrderByDescending(lr => lr.DateSubmitted)
                    .Select(lr => new LeaveRequestResponseDto
                    {
                        LeaveRequestId = lr.LeaveRequestId,
                        EmployeeId = lr.EmployeeId,
                        EmployeeName = lr.Employee.Name,
                        LeaveType = lr.LeaveType,
                        StartDate = lr.StartDate,
                        EndDate = lr.EndDate,
                        Reason = lr.Reason,
                        Status = lr.Status,
                        AdminRemark = lr.AdminRemark,
                        DateSubmitted = lr.DateSubmitted,
                        TotalDays = (lr.EndDate - lr.StartDate).Days + 1,

                    })
                    .ToListAsync();
                return new ServiceResponse<List<LeaveRequestResponseDto>>
                {
                    Success = true,
                    Message = "Leave history retrieved Successfully",
                    Data = leaveHistory
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get leave history error: {ex.Message}");
                return new ServiceResponse<List<LeaveRequestResponseDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve leave history "
                };

            }
        }

    }
}