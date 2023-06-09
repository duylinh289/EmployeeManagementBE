using AutoMapper;
using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Employee;
using EmployeeManagementBE.DTO.Student;
using EmployeeManagementBE.DTO.User;
using EmployeeManagementBE.IRepository;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;
using RepositoryCodeFirstCore.Migrations;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static RabbitMQ.Client.Logging.RabbitMqClientEventSource;

namespace EmployeeManagementBE.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public EmployeeRepository(MyDbContext context, IMapper mapper, IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        #region methods
        public async Task<Guid> Create(CreateUpdateEmployeeDTO req, string userCreate)
        {
            try
            {
                //create employee
                Employee emp = new Employee();
                emp.EmployeeName = req.EmployeeName;
                emp.Email = req.Email;
                emp.DateOfBirth = req.DateOfBirth;
                emp.Gender = req.Gender == "true" ? true : false;
                emp.Status = 1;
                emp.CretedOn = DateTime.Now;
                emp.CreatedBy = userCreate;

                _context.Employees!.Add(emp);
                await _context.SaveChangesAsync();

                //create account
                UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
                userRegisterDTO.Email = req.Email;
                userRegisterDTO.UserName = req.Email;
                userRegisterDTO.Password = "P123456@";
                await _userRepository.Register(userRegisterDTO, 1);

                return emp.EmployeeCode;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        public async Task<string> Delete(Guid req, string userDelete)
        {
            Employee emp = await _context.Employees.Where(x => x.EmployeeCode == req && x.Status == 1).FirstOrDefaultAsync();
            if (emp == null)
            {
                return "Not found";
            }
            else
            {
                emp.Status = 2;
                emp.ModifiedOn = DateTime.Now;
                emp.ModifiedBy = userDelete;

                await _context.SaveChangesAsync();

                return "Success";
            }
        }

        public async Task<List<EmployeeDTO>> GetAll()
        {
            var lsEmployee = await _context.Employees!.Where(e => e.Status == 1).ToListAsync();
            return _mapper.Map<List<Employee>, List<EmployeeDTO>>(lsEmployee);
        }

        public async Task<EmployeeDTO> GetById(Guid req)
        {
            var emp = await _context.Employees!.Where(x => x.EmployeeCode == req).FirstOrDefaultAsync();
            return _mapper.Map<EmployeeDTO>(emp);
        }


        public async Task<string> Update(CreateUpdateEmployeeDTO req, string userModify)
        {
            Employee emp = await _context.Employees.Where(x => x.EmployeeCode == req.EmployeeCode && x.Status == 1).FirstOrDefaultAsync();
            if (emp == null)
            {
                return "Not found";
            }
            else
            {
                emp.EmployeeName = req.EmployeeName;
                emp.ModifiedOn = DateTime.Now;
                emp.ModifiedBy = userModify;
                emp.DateOfBirth = req.DateOfBirth;
                emp.Gender = req.Gender == "true" ? true : false;
                emp.Email = req.Email;

                await _context.SaveChangesAsync();

                return "Success";
            }
        }

        public async Task<List<EmployeeDTO>> Search(string search)
        {
            var employees = await _context.Employees.Where(x => x.Status == 1).ToListAsync();
            var results = employees.Where(e => e.EmployeeCode.Equals(search)
                                        || RemoveSign4VietnameseString(e.EmployeeName).ToLower().Contains(RemoveSign4VietnameseString(search).ToLower())
                                        || search == "all").ToList();

            return _mapper.Map<List<Employee>, List<EmployeeDTO>>(results);
        }

        public async Task<List<ImportEmployeeDTO>> CheckDataImport(List<ImportEmployeeDTO> req)
        {
            var listEmail = await _context.Employees.Where(x => x.Status == 1).Select(x => x.Email).ToListAsync();

            //kiểm tra tên
            req.Where(e => !Regex.IsMatch(e.EmployeeName, namePattern)).ToList().ForEach(e => e.Error = "Name is invalid");

            //kiểm tra email
            req.Where(e => !Regex.IsMatch(e.Email, emailPattern)).ToList().ForEach(e => e.Error = e.Error == "" ? "Email is invalid" : e.Error + " - " + "Email is invalid");

            //kiểm tra gender
            req.Where(e => e.Gender != "0" && e.Gender != "1").ToList().ForEach(e => e.Error = e.Error == "" ? "Gender is invalid" : e.Error + " - " + "Gender is invalid");

            //Kiểm tra email đã được sử dụng chưa
            req.ForEach(e =>
            {
                if (listEmail.Contains(e.Email))
                {
                    e.Error = e.Error == "" ? "Email used" : e.Error + " - " + "Email used";
                }
            });

            //kiểm tra duplicate
            var dupList = req.GroupBy(a => new {a.Email })
                        .Where(g => g.Count() > 1)
                       .Select(g => new {Email = g.Key.Email});

            req.Where(e => dupList.FirstOrDefault(d =>d.Email == e.Email) != null).ToList().ForEach(e => e.Error = e.Error == "" ? "Data is duplicate" : e.Error + " - " + "Data is duplicate");

            return req;
        }


        public async Task<string> SaveImport(List<ImportEmployeeDTO> req, string userName)
        {
            if (!req.Any(x => x.Error != ""))
            {
                //đưa xuống table temp
                List<EmployeeImportTmp> listE = _mapper.Map<List<ImportEmployeeDTO>, List<EmployeeImportTmp>>(req);
                listE.ForEach(x => x.UserImport = userName);
                await _context.EmployeeImportTmp.AddRangeAsync(listE);
                await _context.SaveChangesAsync();

                //gọi store save
                var userNameParameter = new SqlParameter("@user", userName);
                var result = _context.Database.ExecuteSqlRaw("EXEC Employee_Import_Save @user", userNameParameter);
                if (result >= 0)
                {
                    //tạo tài khoản
                    foreach (ImportEmployeeDTO e in req) {
                        UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
                        userRegisterDTO.Email = e.Email;
                        userRegisterDTO.UserName = e.Email;
                        userRegisterDTO.Password = "P123456@";
                        await _userRepository.Register(userRegisterDTO, 1);
                    }
                    return "success";
                }
                else
                {
                    return "Error when exec stored";
                }      
            }
            else
            {
                return "List import have error, check again";
            }
        }
        #endregion

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
