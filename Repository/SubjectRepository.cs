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
        public async Task<string> Create(SubjectDTO req, string usrename)
        {
            Subject sb = new Subject();
            sb.SubjectName = req.SubjectName;
            sb.CreatedOn = DateTime.Now;
            sb.CreateBy = usrename;
            sb.Description = req.Description;

            _context.Subjects.Add(sb);
            await _context.SaveChangesAsync();
            return "success";
            
        }

        public async Task<string> Delete(int req, string username)
        {
            var subj = await _context.Subjects.FirstOrDefaultAsync(x => x.SubjectId == req && x.Status == 1);
            if (subj != null)
            {
                subj.Status = 2;
                subj.ModifiedBy = username;
                subj.ModifiedOn = DateTime.Now;

                await _context.SaveChangesAsync();
                return "success";
            }
            else
            {
                return "not found";
            }
        }

        public async Task<List<SubjectDTO>> GetAll()
        {
            var listSubj = await _context.Subjects.Where(x => x.Status == 1).ToListAsync();
            return _mapper.Map<List<Subject>, List<SubjectDTO>>(listSubj);
        }

        public async Task<List<SubjectDTO>> Search(string keyword)
        {
            var subjs = await _context.Subjects.Where(x => x.Status == 1).ToListAsync();
            var subj = subjs.Where(e => e.SubjectId.ToString().Equals(keyword)
                                        || RemoveSign4VietnameseString(e.SubjectName).ToLower().Contains(RemoveSign4VietnameseString(keyword).ToLower())
                                        || keyword == "all").ToList();

            return _mapper.Map<List<Subject>, List<SubjectDTO>>(subj);
        }

        public async Task<string> Update(SubjectDTO req, string username)
        {
            var subj = await _context.Subjects.FirstOrDefaultAsync(x => x.SubjectId == req.SubjectId && x.Status == 1);
            if (subj != null)
            {
                subj.SubjectName = req.SubjectName;
                subj.Description = req.Description;
                subj.ModifiedBy = username;
                subj.ModifiedOn =  DateTime.Now;


                await _context.SaveChangesAsync();
                return "success";
            }
            else
            {
                return "not found";
            }
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
