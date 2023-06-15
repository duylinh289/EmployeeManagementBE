using EmployeeManagementBE.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using RepositoryCodeFirstCore.Data;

namespace EmployeeManagementBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassStudentController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IClassStudentRepository _repository;
        public ClassStudentController(MyDbContext context, IClassStudentRepository studentRepository)
        {
            _context = context;
            _repository = studentRepository;
        }
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                var res = await _repository.Search(keyword);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent(string classid)
        {
            try
            {
                var res = await _repository.GetStudentOfClass(classid);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetStudentOut")]
        public async Task<IActionResult> GetStudentOut(string classid)
        {
            try
            {
                var res = await _repository.GetStudentOut(classid);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("AddStudent")]
        public async Task<IActionResult> AddStudent(int classid, Guid studentid)
        {
            try
            {
                var res = await _repository.AddStudentToClass(classid, studentid);
                return Ok(new
                {
                    code = "success",
                    message = "Add student to class successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    code = "error",
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("RemoveStudent")]
        public async Task<IActionResult> RemoveStudent(int classid, Guid studentid)
        {
            try
            {
                var res = await _repository.RemoveStudentFromClass(classid, studentid);
                return Ok(new
                {
                    code = "success",
                    message = "Remove student from class successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    code = "error",
                    message = ex.Message
                });
            }
        }
    }
}
