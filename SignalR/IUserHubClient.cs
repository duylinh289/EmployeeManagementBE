namespace EmployeeManagementBE.SignalR
{
    public interface IUserHubClient
    {
        Task SendUser(List<string> user);
    }
}
