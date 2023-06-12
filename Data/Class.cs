using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RepositoryCodeFirstCore.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    public class Class
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ClassId { get; set; }
        [MaxLength(100)]
        [Required]
        public string ClassName { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public int Grade { get; set; }
        [MaxLength(50)]
        public string CreateBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
