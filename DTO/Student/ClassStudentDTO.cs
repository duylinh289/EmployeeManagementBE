namespace EmployeeManagementBE.DTO.Student
{
    public class ClassStudentDTO
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Grade { get; set; }
        public int CountStudents { get; set; } = 0;
    }

    public class AddStudentDTO
    {
        public int ClassId { get; set;}
        public Guid StudentCode { get; set; }
    }
}
