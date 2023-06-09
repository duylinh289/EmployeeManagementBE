using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    [Table("TaskList")]
    public class TaskList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string TaskName { get; set; } = string.Empty;
        [MaxLength(200)]
        public string TaskDescription { get; set; } = string.Empty;
        public Guid Assignee { get; set; }
        [MaxLength(100)]
        public string Reporter { get; set; } = string.Empty;
    }
}
