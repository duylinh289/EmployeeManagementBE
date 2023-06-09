namespace EmployeeManagementBE.SignalR
{
    public interface IMessageHubClient
    {
        Task SendMessage(List<string> message);
    }
}
