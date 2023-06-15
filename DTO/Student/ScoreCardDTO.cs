namespace EmployeeManagementBE.DTO.Student
{
    public class ScoreCardDTO
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Guid? StudentCode { get; set; }
        public float? RegularExam { get; set; }
        public float? MidtermExam { get; set; }
        public float? FinalExam { get; set; }
        public float? AvgScore { get; set; }
    }
}
