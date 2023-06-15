using AutoMapper;
using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Student;
using EmployeeManagementBE.IRepository;
using Microsoft.EntityFrameworkCore;
using RepositoryCodeFirstCore.Data;

namespace EmployeeManagementBE.Repository
{
    public class ClassStudentRepository : IClassStudentRepository
    {
        private readonly IMapper _mapper;
        private readonly MyDbContext _context;
        public ClassStudentRepository(IMapper mapper, MyDbContext context) {
            _mapper = mapper;
            _context = context;
        }
        public async Task<string> AddStudentToClass(int classid, Guid studentcode)
        {
            StudentClass studentDTO = new StudentClass();
            studentDTO.ClassId = classid;
            studentDTO.StudentCode = studentcode;

            _context.StudentClasses.Add(studentDTO);
            await _context.SaveChangesAsync();
            return "success";
        }

        public async Task<List<StudentsDTO>> GetStudentOfClass(string classid)
        {
            var list = await _context.StudentClasses
                .Where(x => x.ClassId.ToString() == classid)
                .Join(
                    _context.Students,
                    sc => sc.StudentCode,
                    s => s.StudentCode,
                    (sc, s) => new StudentsDTO
                    {
                        StudentCode = sc.StudentCode,
                        StudentName = s.StudentName,
                        CreatedOn = s.CreatedOn,
                    })
                .ToListAsync();

            return list;
        }

        public async Task<string> RemoveStudentFromClass(int classid, Guid studentcode)
        {
            StudentClass studentDTO = new StudentClass();
            studentDTO.ClassId = classid;
            studentDTO.StudentCode = studentcode;

            _context.StudentClasses.Remove(studentDTO);
            await _context.SaveChangesAsync();
            return "success";
        }

        public async Task<List<ClassStudentDTO>> Search(string keyword)
        {
            var classes = await _context.Classes.ToListAsync();
            var listClasses = classes
                                    .Where(e => e.ClassId.ToString().Equals(keyword)
                                        || RemoveSign4VietnameseString(e.ClassName).ToLower().Contains(RemoveSign4VietnameseString(keyword).ToLower())
                                        || keyword == "all")
                                    .GroupJoin(
                                        _context.StudentClasses,
                                        c => c.ClassId,
                                        cs => cs.ClassId,
                                        (c, cs) => new ClassStudentDTO
                                        {
                                            ClassId = c.ClassId,
                                            ClassName = c.ClassName,
                                            Description = c.Description,
                                            Grade = c.Grade,
                                            CountStudents = cs.Count()
                                        })
                                        .ToList();

            return listClasses;
        }

        public async Task<List<StudentsDTO>> GetStudentOut(string classid)
        {
            var studentsNotInClass = await _context.Students
                                .Where(s => !_context.StudentClasses.Any(soc => soc.StudentCode == s.StudentCode) && s.Status == 1)
                                .Select(s => new StudentsDTO
                                {
                                    StudentCode = s.StudentCode,
                                    StudentName = s.StudentName
                                })
                                .ToListAsync();

            return studentsNotInClass;
        }

        #region support
        private static readonly string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        private static readonly string namePattern = @"^[AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬBCDĐEÈẺẼÉẸÊỀỂỄẾỆFGHIÌỈĨÍỊJKLMNOÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢPQRSTUÙỦŨÚỤƯỪỬỮỨỰVWXYỲỶỸÝỴZ][aàảãáạăằẳẵắặâầẩẫấậbcdđeèẻẽéẹêềểễếệfghiìỉĩíịjklmnoòỏõóọôồổỗốộơờởỡớợpqrstuùủũúụưừửữứựvwxyỳỷỹýỵz]+ [AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬBCDĐEÈẺẼÉẸÊỀỂỄẾỆFGHIÌỈĨÍỊJKLMNOÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢPQRSTUÙỦŨÚỤƯỪỬỮỨỰVWXYỲỶỸÝỴZ][aàảãáạăằẳẵắặâầẩẫấậbcdđeèẻẽéẹêềểễếệfghiìỉĩíịjklmnoòỏõóọôồổỗốộơờởỡớợpqrstuùủũúụưừửữứựvwxyỳỷỹýỵz]+(?: [AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬBCDĐEÈẺẼÉẸÊỀỂỄẾỆFGHIÌỈĨÍỊJKLMNOÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢPQRSTUÙỦŨÚỤƯỪỬỮỨỰVWXYỲỶỸÝỴZ][aàảãáạăằẳẵắặâầẩẫấậbcdđeèẻẽéẹêềểễếệfghiìỉĩíịjklmnoòỏõóọôồổỗốộơờởỡớợpqrstuùủũúụưừửữứựvwxyỳỷỹýỵz]*)*";
        private static readonly string[] VietnameseSigns = new string[]
            {

                        "aAeEoOuUiIdDyY",

                        "áàạảãâấầậẩẫăắằặẳẵ",

                        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

                        "éèẹẻẽêếềệểễ",

                        "ÉÈẸẺẼÊẾỀỆỂỄ",

                        "óòọỏõôốồộổỗơớờợởỡ",

                        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

                        "úùụủũưứừựửữ",

                        "ÚÙỤỦŨƯỨỪỰỬỮ",

                        "íìịỉĩ",

                        "ÍÌỊỈĨ",

                        "đ",

                        "Đ",

                        "ýỳỵỷỹ",

                        "ÝỲỴỶỸ"
            };
        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        #endregion
    }
}
