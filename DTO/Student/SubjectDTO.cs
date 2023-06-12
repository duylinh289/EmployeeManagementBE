using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementBE.DTO.Student
{
    public class SubjectDTO
    {
        public int  SubjectId { get; set; } = 0;
        public string SubjectName { get; set; } = string.Empty;
        public string? CreateBy { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
    }
}
