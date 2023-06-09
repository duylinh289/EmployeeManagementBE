using EmployeeManagementBE.DTO.Student;

namespace RepositoryCodeFirstCore.IRepository
{
    public interface IStudentsRepository
    {
        public Task<List<StudentsDTO>> GetAll();
        public Task<StudentsDTO> GetById(Guid req);
        public Task<Guid> CreateStudent(CreateStudentDTO req);
        public Task<string> UpdateStudent(UpdateStudentDTO req);
        public Task<string> DeleteStudent(Guid req);
    }
}
