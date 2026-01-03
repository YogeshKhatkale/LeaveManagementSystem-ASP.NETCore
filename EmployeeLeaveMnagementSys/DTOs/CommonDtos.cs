namespace EmployeeLeaveManagementSys.DTOs
{
    public class ServiceResponse
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public string? Status { get; set;}
    }

    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
