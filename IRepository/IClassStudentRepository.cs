using EmployeeManagementBE.DTO.Student;

namespace EmployeeManagementBE.IRepository
{
    public interface IClassStudentRepository
    {
        public Task<List<ClassStudentDTO>> Search(string keyword);
        public Task<List<StudentsDTO>> GetStudentOfClass(string classid);
        public Task<string> AddStudentToClass(int classid, Guid studentcode);
        public Task<string> RemoveStudentFromClass(int classid, Guid studentcode);
        public Task<List<StudentsDTO>> GetStudentOut(string classid);
    }
}
