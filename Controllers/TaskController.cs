using EmployeeManagementBE.DTO.User;
using EmployeeManagementBE.DTO;
using EmployeeManagementBE.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryCodeFirstCore.Data;
using EmployeeManagementBE.DTO.Task;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeManagementBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITaskRepository _taskRepository;

        public TaskController(MyDbContext context, ITaskRepository taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var listTask = await _taskRepository.GetAll();
                return Ok(listTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO
                {
                    code = "error",
                    message = ex.Message.ToString(),
                });
            }
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,ManagerLV1,ManagerLV2")]
        public async Task<ActionResult<ResponseDTO>> Assign(TaskAssignDTO req)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                req.Reporter = userid;
                string result = await _taskRepository.AssignToTask(req);
                if (result == "Success")
                {
                    return Ok(new ResponseDTO
                    {
                        code = "success",
                        message = "Assign successfully!"
                    });
                }
                else
                {
                    return NotFound(new ResponseDTO
                    {
                        code = "not found",
                        message = "Can not found this task!"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO
                {
                    code = "error",
                    message = ex.Message.ToString(),
                });
            }
        }

    }
}
