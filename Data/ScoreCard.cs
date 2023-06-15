using RepositoryCodeFirstCore.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementBE.Data
{
    public class ScoreCard
    {
        [Key]
        [Column(Order = 1)]
        public Guid StudentCode { get; set; }
        public Student Student { get; set; }
        [Key]
        [Column(Order = 2)]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public float? RegularExam { get; set; }
        public float? MidtermExam { get; set; }
        public float? FinalExam { get; set; }
        public float? AvgScore { get; set; }
    }
}
