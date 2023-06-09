using EmployeeManagementBE.DTO;
using EmployeeManagementBE.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EmployeeManagementBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushMessageController : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHubClient> messageHub;
        private IHubContext<UserHub, IUserHubClient> userHub;
        public PushMessageController(IHubContext<MessageHub, IMessageHubClient> _messageHub, IHubContext<UserHub, IUserHubClient> userHub)
        {
            messageHub = _messageHub;
            this.userHub = userHub;
        }
        [HttpPost]
        [Route("push")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> Push(string message)
        {
            try
            {
                List<string> messages = new List<string>();
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var role = User.FindFirstValue(ClaimTypes.Role);

                messages.Add(DateTime.Now.ToString("HH:mm:ss tt") + " - " + userid + "(" + role + "): " + message);

                await messageHub.Clients.All.SendMessage(messages);
                return Ok(new ResponseDTO
                {
                    code = "success",
                    message = "Push successfully!"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseDTO
                {
                    code = "error",
                    message = ex.Message.ToString()
                });
            }
        }

        [HttpPost]
        [Route("user")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> PushUser()
        {
            try
            {
                List<string> users = new List<string>();
                var userid = User.FindFirstValue(ClaimTypes.Name);
                var role = User.FindFirstValue(ClaimTypes.Role);

                users.Add(userid + "(" + role + ")");

                await userHub.Clients.All.SendUser(users);
                return Ok(new ResponseDTO
                {
                    code = "success",
                    message = "Push successfully!"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseDTO
                {
                    code = "error",
                    message = ex.Message.ToString()
                });
            }
        }

    }
}
