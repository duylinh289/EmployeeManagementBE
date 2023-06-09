using AutoMapper;
using EmployeeManagementBE.DTO.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;

namespace RepositoryCodeFirstCore.Repository
{
    public class StudentsRepository : IStudentsRepository
    {	
		private readonly MyDbContext _context;
		private readonly IMapper _mapper;
		public StudentsRepository(MyDbContext context, IMapper mapper) {
			_context = context;
			_mapper = mapper;
		}

        public async Task<Guid> CreateStudent(CreateStudentDTO req)
        {
			Student st = new Student();
			st.StudentName = req.StudentName;
			st.CreatedOn = DateTime.Now;

			_context.Students!.Add(st);
			await _context.SaveChangesAsync();
			return st.StudentCode;
        }

        public async Task<string> DeleteStudent(Guid req)
        {
            var st = await _context.Students!.Where(x => x.StudentCode == req).FirstOrDefaultAsync();
            if (st == null)
            {
                return "Not found";
            }
            else
            {
                _context.Students!.Remove(st);
                await _context.SaveChangesAsync();
                return "Success";
            }    
        }

        public async Task<List<StudentsDTO>> GetAll()
        {
            var lsStudents = await _context.Students!.ToListAsync();
            return _mapper.Map<List<Student>, List<StudentsDTO>>(lsStudents);
        }

        public async Task<StudentsDTO> GetById(Guid req)
        {
			var st = await _context.Students!.Where(x => x.StudentCode == req).FirstOrDefaultAsync();
            return _mapper.Map<StudentsDTO>(st);	
        }

        public async Task<string> UpdateStudent(UpdateStudentDTO req)
        {

			var st = await _context.Students!.Where(x => x.StudentCode == req.StudentCode).FirstOrDefaultAsync();
            if (st == null)
            {
                return "Not found";
            }
            else
            {
                st.StudentName = req.StudentName;
                st.ModifiedOn = DateTime.Now;
                await _context.SaveChangesAsync();

                return "Success";
            }			
        }
    }
}
