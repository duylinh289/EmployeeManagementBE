using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    [Table("AssignToTask")]
    public class AssignToTask
    {
        [Key]
        public Guid EmployeeId { get; set; }
        [Key]
        public int TaskId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Reporter { get; set; } = string.Empty;
    }
}
