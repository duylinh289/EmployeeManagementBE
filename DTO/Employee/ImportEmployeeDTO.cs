namespace EmployeeManagementBE.DTO.Employee
{
    public class ImportEmployeeDTO
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Error { get; set; } = string.Empty;
    }

}
