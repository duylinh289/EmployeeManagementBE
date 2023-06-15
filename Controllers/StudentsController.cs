using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagementBE.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;

namespace RepositoryCodeFirstCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IStudentsRepository _studentsRepository;
        public StudentsController(MyDbContext context, IStudentsRepository studentsRepository)
        {
            _context = context;
            _studentsRepository = studentsRepository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _studentsRepository.GetAll();
                return res != null ? Ok(res) : NotFound();
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
                var res = await _studentsRepository.Search(req);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("SearchByCondition")]
        public async Task<IActionResult> SearchByCondition(SearchStudentDTO req)
        {
            try
            {
                var res = await _studentsRepository.SearchByCondition(req);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var res = await _studentsRepository.GetById(id);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("Create")]
        public async Task<IActionResult> CreateStudent(CreateStudentDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var res = await _studentsRepository.CreateStudent(req, userid);
                var resF = new
                {
                    code = "Success",
                    message = "Successfully Created Student" + res
                };
                return Ok(resF);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        [Authorize]
        [Route("Update")]
        public async Task<IActionResult> UpdateStudent(UpdateStudentDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                string res = await _studentsRepository.UpdateStudent(req, userid);
                return res == "Success" ? Ok(new
                                            {
                                                code = "Success",
                                                message = "Successfully Created Student" + req.StudentCode.ToString()
                                            }) 
                                        : NotFound(new
                                        {
                                            code = "Not found",
                                            message = "Cannot Found Student " + req.StudentCode.ToString()
                                        });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        [Authorize]
        [Route("Delete")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                string res = await _studentsRepository.DeleteStudent(id, userid);
                return res == "Success" ? Ok(new
                {
                    code = "Success",
                    message = "Successfully Deleted Student" + id.ToString()
                })
                                        : NotFound(new
                                        {
                                            code = "Not found",
                                            message = "Cannot Found Student " + id.ToString()
                                        });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetScoreCard")]
        public async Task<IActionResult> GetScoreCard(Guid studentcode)
        {
            try
            {
                var res = await _studentsRepository.GetScoreCard(studentcode);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetSubject")]
        public async Task<IActionResult> GetSubject(Guid studentcode)
        {
            try
            {
                var res = await _studentsRepository.GetSubjectForRegis(studentcode);
                return res != null ? Ok(res) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("AddSubject")]
        public async Task<IActionResult> AddSubject(int subjectid, Guid studentid)
        {
            try
            {
                var res = await _studentsRepository.RegisSubject(studentid, subjectid);
                return Ok(new
                {
                    code = "success",
                    message = "Regis subject successfully"
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
        [Route("RemoveSubject")]
        public async Task<IActionResult> RemoveSubject(int subjectid, Guid studentid)
        {
            try
            {
                var res = await _studentsRepository.RemoveSubject(studentid, subjectid);
                return Ok(new
                {
                    code = "success",
                    message = "Remove subject successfully"
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
        [Route("EditScore")]
        public async Task<IActionResult> EditScore(ScoreCardDTO req)
        {
            try
            {
                var res = await _studentsRepository.EditScoreCard(req);
                return Ok(new
                {
                    code = "success",
                    message = "Edit Score successfully"
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
