using EmployeeManagementBE.Data;
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
        public Task<List<StudentsDTO>> Search(string keyword);
        public Task<List<Students_RankDTO>> SearchByCondition(SearchStudentDTO req);
        public Task<List<SubjectDTO>> GetSubjectForRegis(Guid studentcode);
        public Task<List<ScoreCardDTO>> GetScoreCard(Guid studentcode);
        public Task<string> RegisSubject(Guid studentcode, int subjectid);
        public Task<string> RemoveSubject(Guid studentcode, int subjectid);
        public Task<string> EditScoreCard(ScoreCardDTO req);
    }
}
