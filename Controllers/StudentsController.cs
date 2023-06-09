using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementBE.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("Create")]
        public async Task<IActionResult> CreateStudent(CreateStudentDTO req)
        {
            try
            {
                var res = await _studentsRepository.CreateStudent(req);
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
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateStudent(UpdateStudentDTO req)
        {
            try
            {
                string res = await _studentsRepository.UpdateStudent(req);
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
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                string res = await _studentsRepository.DeleteStudent(id);
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
    }
}
