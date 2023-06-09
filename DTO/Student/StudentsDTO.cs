using Microsoft.Build.Framework;

namespace EmployeeManagementBE.DTO.Student
{
    public class StudentsDTO
    {
        public Guid StudentCode { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
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
