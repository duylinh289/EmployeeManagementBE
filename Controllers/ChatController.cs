using EmployeeManagementBE.SignalR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementBE.DTO.SignalR;
using EmployeeManagementBE.DTO.User;
using Microsoft.AspNetCore.SignalR;
using EmployeeManagementBE.SignalR.Hubs;
using EmployeeManagementBE.DTO;

namespace EmployeeManagementBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;


        public ChatController(ChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserDTO model)
        {
            if (_chatService.AddUserToList(model.UserName))
            {
                return NoContent();
            }
            return BadRequest("Please input name other");
        }
        [HttpPost("task-assign")]
        public async Task<IActionResult> AssignTask([FromBody] TaskAssignmentModel model)
        {
            await _hubContext.Clients.User(model.userName).SendAsync("ReceiveTaskAssignment", model.task);

            return Ok();
        }
        [HttpGet("getuser")]
        public async Task<IActionResult> GetAllUser()
        {
            var l = _chatService.GetOnlineUsers();

            return Ok(l);
        }

        [HttpGet("checkonline")]
        public async Task<ActionResult<ResponseDTO>> CheckOnline(string user)
        {
            var l = _chatService.GetConnectionIdByUser(user);

            return Ok(new ResponseDTO
            {
                code = l == null? "offline" : "online",
                message = l
            }); 
        }


    }
}
