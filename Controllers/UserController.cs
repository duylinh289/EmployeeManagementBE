using EmployeeManagementBE.DTO;
using EmployeeManagementBE.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using RepositoryCodeFirstCore.Data;
using RepositoryCodeFirstCore.IRepository;
using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;

namespace RepositoryCodeFirstCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IUserRepository _userRepository;
        public UserController (MyDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ResponseDTO>> Register([FromBody] UserRegisterDTO user) {
            try
            {
                var result = await _userRepository.Register(user);
                return Ok(new ResponseDTO
                {
                    code = "success",
                    message = "Account successfully created!"
                });
            }
            catch(Exception ex)
            {
                return Ok(new ResponseDTO
                {
                    code = "error",
                    message = ex.Message.ToString(),
                });
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> Login([FromBody] UserLoginDTO user)
        {
            try
            {
                var username = HttpContext.Session.GetString(user.UserName);
                if (!string.IsNullOrEmpty(username))
                {
                    return BadRequest(new ResponseDTO
                    {
                        code = "logged",
                        message = "User has been logged in another client!"
                    });
                }

                string result = await _userRepository.Login(user);
                if (result == "Not found")
                {
                    return NotFound(new ResponseDTO
                    {
                        code = "not found",
                        message = "User name or password is incorrect!"
                    });
                }
                else
                {
                    HttpContext.Session.SetString(user.UserName, user.UserName);
                    return Ok(new ResponseDTO
                    {
                        code = "success",
                        message = result
                    });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseDTO
                {
                    code = "error",
                    message = ex.Message.ToString(),
                });
            }
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> LogOut()
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.Name);
                HttpContext.Session.Remove(userid);
                return Ok(new ResponseDTO
                {
                    code = "success",
                    message = "Loged out"
                });
            }
            catch (Exception)
            {
                return BadRequest(new ResponseDTO
                {
                    code = "error",
                    message = "error when loging out",
                });
            }
        }

        [HttpPost]
        [Route("update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO>> Update(UserDTO req)
        {
            try
            {
                await _userRepository.Update(req);

                return Ok(new ResponseDTO
                {
                    code = "success",
                    message = "Update user's infomation successfully!"
                });
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
        [HttpPut]
        [Route("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO>> Delete(UserDTO req)
        {
            try
            {
                await _userRepository.Delete(req.UserName);

                return Ok(new ResponseDTO
                {
                    code = "success",
                    message = "Delete user successfully!"
                });
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

        [HttpGet]
        [Route("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO>> GetAll ()
        {
            try
            {
                var listUser = await _userRepository.GetAll();

                return Ok(listUser);
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
        [HttpGet]
        [Route("get-user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO>> GetUser(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsername(username);

                return Ok(user);
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
