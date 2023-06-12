using EmployeeManagementBE.DTO.Student;

namespace EmployeeManagementBE.IRepository
{
    public interface ISubjectRepository
    {
        public Task<List<SubjectDTO>> GetAll();
        public Task<string> Create(SubjectDTO req);
        public Task<string> Update(SubjectDTO req);
        public Task<string> Delete(SubjectDTO req);
    }
}
