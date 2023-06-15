using AutoMapper;
using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Employee;
using EmployeeManagementBE.DTO.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        public async Task<Guid> CreateStudent(CreateStudentDTO req, string username)
        {
			Student st = new Student();
			st.StudentName = req.StudentName;
			st.CreatedOn = DateTime.Now;
            st.CreateBy = username;

			_context.Students!.Add(st);
			await _context.SaveChangesAsync();
			return st.StudentCode;
        }

        public async Task<string> DeleteStudent(Guid req, string username)
        {
            var st = await _context.Students!.Where(x => x.StudentCode == req).FirstOrDefaultAsync();
            if (st == null)
            {
                return "Not found";
            }
            else
            {
                st.Status = 2;
                st.ModifiedBy = username;
                st.ModifiedOn = DateTime.Now;
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

        public async Task<List<StudentsDTO>> Search(string keyword)
        {
            var students = await _context.Students.Where(x => x.Status == 1).ToListAsync();
            var results = students.Where(e => e.StudentCode.ToString().Equals(keyword)
                                        || RemoveSign4VietnameseString(e.StudentName).ToLower().Contains(RemoveSign4VietnameseString(keyword).ToLower())
                                        || keyword == "all").ToList();

            return _mapper.Map<List<Student>, List<StudentsDTO>>(results);
        }
        public async Task<List<Students_RankDTO>> SearchByCondition(SearchStudentDTO req)
        {
            //gọi store save
            var studentKeyWord = new SqlParameter("@studentKeyWord", req.Student);
            var classP = new SqlParameter("@class", req.Class);
            var rankP = new SqlParameter("@rank", req.Rank);
            var result = await _context.StudentRank
                                       .FromSqlRaw("EXEC Student_Info_Select @studentKeyWord, @class, @rank",
                                                   studentKeyWord, classP, rankP)
                                       .ToListAsync();


            return  result;
        }

        public async Task<string> UpdateStudent(UpdateStudentDTO req, string username)
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
                st.ModifiedBy = username;
                await _context.SaveChangesAsync();

                return "Success";
            }			
        }

        public async Task<List<SubjectDTO>> GetSubjectForRegis(Guid studentcode)
        {
            var subs = await _context.Subjects
                    .Where(s => !_context.ScoreCards.Any(soc => soc.SubjectId == s.SubjectId && soc.StudentCode == studentcode))
                    .Select(s => new SubjectDTO
                    {
                        SubjectId = s.SubjectId,
                        SubjectName = s.SubjectName
                    })
                    .ToListAsync();

            return subs;
        }

        public async Task<List<ScoreCardDTO>> GetScoreCard(Guid studentcode)
        {
            var list = await _context.ScoreCards
                        .Where(x => x.StudentCode == studentcode)
                        .Join(
                            _context.Subjects,
                            sc => sc.SubjectId,
                            s => s.SubjectId,
                            (sc, s) => new ScoreCardDTO
                            {
                                SubjectId = sc.SubjectId,
                                SubjectName = s.SubjectName,
                                RegularExam = sc.RegularExam,
                                MidtermExam = sc.MidtermExam,
                                FinalExam = sc.FinalExam,
                                AvgScore = sc.AvgScore,
                            })
                        .ToListAsync();

            return list;
        }

        public async Task<string> RegisSubject(Guid studentcode, int subjectid)
        {
            ScoreCard score = new ScoreCard();
            score.SubjectId = subjectid;
            score.StudentCode = studentcode;

            _context.ScoreCards.Add(score);
            await _context.SaveChangesAsync();
            return "success";
        }

        public async Task<string> RemoveSubject(Guid studentcode, int subjectid)
        {
            ScoreCard score = new ScoreCard();
            score.SubjectId = subjectid;
            score.StudentCode = studentcode;

            _context.ScoreCards.Remove(score);
            await _context.SaveChangesAsync();
            return "success";
        }

        public async Task<string> EditScoreCard(ScoreCardDTO req)
        {
            ScoreCard score = await _context.ScoreCards.FirstOrDefaultAsync(x => x.StudentCode == req.StudentCode && x.SubjectId == req.SubjectId);

            score.RegularExam = req.RegularExam;
            score.FinalExam = req.FinalExam;
            score.MidtermExam = req.MidtermExam;
            score.AvgScore = (score.RegularExam + score.MidtermExam * 2 + score.FinalExam * 3)/6;

            await _context.SaveChangesAsync();
            return "success";
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
