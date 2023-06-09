using Microsoft.AspNetCore.SignalR;

namespace EmployeeManagementBE.SignalR
{
    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task SendMessage(List<string> message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}
