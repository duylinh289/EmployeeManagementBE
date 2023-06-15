using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementBE.DTO.Student
{
    public class StudentsDTO
    {
        public Guid StudentCode { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string CreateBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
    }

    public class SearchStudentDTO
    {
        public string? Student { get; set; }
        public string? Class { get; set; }
        public string? Rank { get; set; }
    }

    public class CreateStudentDTO
    {
        [Required]
        public string StudentName { get; set; } = string.Empty;
    }
    public class UpdateStudentDTO
    {
        [Required]
        public Guid StudentCode { get; set; }
        [Required]
        public string StudentName { get; set; } = string.Empty;
    }
}
