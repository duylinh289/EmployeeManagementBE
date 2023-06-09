using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagementBE.DTO.Employee
{
    public class CreateUpdateEmployeeDTO
    {
        [JsonPropertyName("employeeCode")]
        public Guid EmployeeCode { get; set; } = Guid.NewGuid();
        [Required]
        [JsonPropertyName("employeeName")]
        public string EmployeeName { get; set; } = string.Empty;
        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

    }
}
