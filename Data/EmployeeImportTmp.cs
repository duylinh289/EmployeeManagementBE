using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    [Table("EmployeeImportTmp")]
    public class EmployeeImportTmp
    {
        [Key] public int Id { get; set; }
        [MaxLength(300)]
        public string EmployeeName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(5)]
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string UserImport { get; set; } = string.Empty;
    }
}
