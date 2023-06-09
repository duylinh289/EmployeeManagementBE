using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [Required]
        public Guid EmployeeCode { get; set; }
        [Required]
        [MaxLength(200)]
        public string EmployeeName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        public bool Gender { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CretedOn { get; set; }
        [MaxLength(50)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set;}
        [Required]
        public int Status { get; set; }

    }
}
