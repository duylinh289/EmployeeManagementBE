using EmployeeManagementBE.DTO.Student;

namespace RepositoryCodeFirstCore.IRepository
{
    public interface IStudentsRepository
    {
        public Task<List<StudentsDTO>> GetAll();
        public Task<StudentsDTO> GetById(Guid req);
        public Task<Guid> CreateStudent(CreateStudentDTO req, string username);
        public Task<string> UpdateStudent(UpdateStudentDTO req, string username);
        public Task<string> DeleteStudent(Guid req, string username);
    }
}
