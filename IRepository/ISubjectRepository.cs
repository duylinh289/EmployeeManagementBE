using EmployeeManagementBE.DTO.Student;

namespace EmployeeManagementBE.IRepository
{
    public interface ISubjectRepository
    {
        public Task<List<SubjectDTO>> GetAll();
        public Task<string> Create(SubjectDTO req, string username);
        public Task<string> Update(SubjectDTO req, string username);
        public Task<string> Delete(int req, string username);
        public Task<List<SubjectDTO>> Search(string keyword);
    }
}
