using EmployeeManagementBE.DTO.Employee;
using EmployeeManagementBE.DTO.Student;
using EmployeeManagementBE.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryCodeFirstCore.Data;
using System.Data;
using System.Security.Claims;

namespace EmployeeManagementBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ISubjectRepository _repository;
        public SubjectController(MyDbContext context, ISubjectRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _repository.GetAll();
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(string req)
        {
            try
            {
                var res = await _repository.Search(req);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,ManagerLV1")]
        [Route("Create")]
        public async Task<IActionResult> Create(SubjectDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var res = await _repository.Create(req, userid);
                var resF = new
                {
                    code = "Success",
                    message = "Successfully Created Subject: " + req.SubjectName
                };
                return Ok(resF);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPut]
        [Authorize(Roles = "Admin,ManagerLV1")]
        [Route("Update")]
        public async Task<IActionResult> Update(SubjectDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                string res = await _repository.Update(req, userid);
                return res == "success" ? Ok(new
                {
                    code = "Success",
                    message = "Successfully Updated Subject: " + req.SubjectId.ToString()
                })
                                        : BadRequest(new
                                        {
                                            code = "Not found",
                                            message = "Cannot Found Subject " + req.SubjectId.ToString()
                                        });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPut]
        [Authorize(Roles = "Admin,ManagerLV1")]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                string res = await _repository.Delete(req, userid);
                return res == "Success" ? Ok(new
                {
                    code = "Success",
                    message = "Successfully Deleted Subject"
                })
                                        : BadRequest(new
                                        {
                                            code = "Not found",
                                            message = "Cannot Found Employee " + req.ToString()
                                        });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
