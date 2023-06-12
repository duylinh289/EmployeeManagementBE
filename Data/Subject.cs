using RepositoryCodeFirstCore.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    public class Subject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int SubjectId { get; set; }
        [Required]
        [MaxLength(100)]
        public string SubjectName { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string CreateBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
    }
}
