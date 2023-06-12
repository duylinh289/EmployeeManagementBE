using AutoMapper;
using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Student;
using EmployeeManagementBE.IRepository;
using Microsoft.EntityFrameworkCore;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;

namespace EmployeeManagementBE.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public SubjectRepository(MyDbContext context, IMapper mapper, IStudentsRepository studentsRepository)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> Create(SubjectDTO req)
        {
            Subject sb = new Subject();
            sb.SubjectName = req.SubjectName;

            await _context.Subjects.AddAsync(sb);
            return "success";
            
        }

        public Task<string> Delete(SubjectDTO req)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubjectDTO>> GetAll()
        {
            var listSubj = await _context.Subjects.Where(x => x.Status == 1).ToListAsync();
            return _mapper.Map<List<Subject>, List<SubjectDTO>>(listSubj);
        }

        public async Task<string> Update(SubjectDTO req)
        {
            var subj = await _context.Subjects.FirstOrDefaultAsync(x => x.SubjectId == req.SubjectId && x.Status == 1);
            if (subj != null)
            {
                subj.SubjectName = req.SubjectName;
                
                _context.SaveChangesAsync();
                return "success";
            }
            else
            {
                return "not found";
            }
        }
    }
}
