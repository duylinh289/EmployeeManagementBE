using Microsoft.AspNetCore.SignalR;
namespace EmployeeManagementBE.SignalR
{
    public class UserHub : Hub<IUserHubClient>
    {
        public async Task SendUser(List<string> user)
        {
            await Clients.All.SendUser(user);
        }
    }
}
