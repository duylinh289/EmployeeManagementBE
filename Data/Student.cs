﻿using EmployeeManagementBE.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryCodeFirstCore.Data
{
    [Table("Student")]
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid StudentCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string StudentName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string CreateBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }
        public int Status { get; set; } = 1;
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
