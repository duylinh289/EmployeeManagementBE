using RepositoryCodeFirstCore.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EmployeeManagementBE.Data
{
    public class StudentClass
    {
        [Key]
        [Column(Order = 1)]
        public Guid StudentCode { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ClassId { get; set; }
        public Student Student { get; set; }
        public Class Class { get; set; }    
        public int SchoolYear { get; set; }
    }
}
