using EmployeeManagementBE.DTO.Employee;
using EmployeeManagementBE.DTO.Student;
using EmployeeManagementBE.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;
using RepositoryCodeFirstCore.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManagementBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IEmployeeRepository _repository;
        public EmployeeController(MyDbContext context, IEmployeeRepository repository) {
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
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var res = await _repository.GetById(id);
                return res != null ? Ok(res) : NotFound("Can not found employee!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search(string req)
        {
            try
            {
                var res = await _repository.Search(req);
                return res != null ? Ok(res) : NotFound("Can not found employee!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,ManagerLV1")]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateUpdateEmployeeDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var res = await _repository.Create(req, userid);
                var resF = new
                {
                    code = "Success",
                    message = "Successfully Created Employee: " + res
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
        public async Task<IActionResult> Update(CreateUpdateEmployeeDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                string res = await _repository.Update(req, userid);
                return res == "Success" ? Ok(new
                {
                    code = "Success",
                    message = "Successfully Updated Employee: " + req.EmployeeCode.ToString()
                })
                                        : BadRequest(new
                                        {
                                            code = "Not found",
                                            message = "Cannot Found Employee " + req.EmployeeCode.ToString()
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                string res = await _repository.Delete(id, userid);
                return res == "Success" ? Ok(new
                {
                    code = "Success",
                    message = "Successfully Deleted Employee: " + id.ToString()
                })
                                        : BadRequest(new
                                        {
                                            code = "Not found",
                                            message = "Cannot Found Employee " + id.ToString()
                                        });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ManagerLV1")]
        [Route("Check-import")]
        public async Task<IActionResult> CheckDataImport(List<ImportEmployeeDTO> req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var result = await _repository.CheckDataImport(req);
                return Ok(new
                {
                    code = "Success",
                    message = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    code = "Success",
                    message = ex.Message.ToString()
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ManagerLV1")]
        [Route("Save-import")]
        public async Task<IActionResult> SaveImport(List<ImportEmployeeDTO> req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var result = await _repository.SaveImport(req, userid);
                if (result == "success")
                {
                    return Ok(new
                    {
                        code = "Success",
                        message = result
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "Error",
                        message = result
                    });
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    code = "Success",
                    message = ex.Message.ToString()
                });
            }
        }
    }
}
