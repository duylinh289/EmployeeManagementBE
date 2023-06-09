using EmployeeManagementBE.SignalR.Services;
using Microsoft.AspNetCore.SignalR;
using EmployeeManagementBE.DTO.SignalR;

namespace EmployeeManagementBE.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }
        public override async Task OnConnectedAsync()
        {
            //tạo room tên 'SNPGroup'
            await Groups.AddToGroupAsync(Context.ConnectionId, "SNPGroup");
            //await Clients.Caller.SendAsync("UserConnected");
        } //gọi lúc create connection
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SNPGroup");
            //remove user noti
            var userNoti = _chatService.GetUserNotiByConnectionId(Context.ConnectionId);
            _chatService.RemoveUserNoti(userNoti);

            await base.OnDisconnectedAsync(exception);
        } //gọi lúc close connection

        #region Chat room
        public async Task JoinRoomChat() //join vào room khi mở màn hình chat
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SNPGroup");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public async Task LeaveRoomChat(string user) //out khỏi room khi thoát khỏi màn hình chat
        {
            _chatService.RemoveUserFromList(user);
            // show lại danh sách user sau khi 1 user đã out
            await DisplayOnlineUsers();
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatService.AddUserConnectionId(name, Context.ConnectionId);
            await DisplayOnlineUsers();
        } //update connectionId ứng với user đang trong group

        private async Task DisplayOnlineUsers()
        {
            var onlineUsers = _chatService.GetOnlineUsers();
            await Clients.Groups("SNPGroup").SendAsync("OnlineUsers", onlineUsers);
        } //get danh sách user đang trong group

        public async Task ReceiveMessage(Message message)
        {
            message.Time = DateTime.Now.ToString("HH:mm:ss tt");
            await Clients.Group("SNPGroup").SendAsync("NewMessage", message);
        } //send message

        #endregion

        #region Chat riêng/gửi thông báo
        public async Task CreatePrivateChat(Message message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            //await Groups.AddToGroupAsync(message.From, privateGroupName);
            //var toConnectionId = _chatService.GetConnectionIdByUser(message.To);
            var toConnectionId = _chatService.GetNotiConnectionIdByUser(message.To);
            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

            await Clients.Client(toConnectionId).SendAsync("OpenPrivate", message);
        } // tạo group riêng

        public async Task RecivePrivateMessage(Message message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
        } // gửi message đến group riêng

        public async Task RemovePrivateMessage(string from, string to)
        {
            string privateGroupName = GetPrivateGroupName(from, to);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            //await Groups.RemoveFromGroupAsync(from, privateGroupName);
            var toConnectionId = _chatService.GetConnectionIdByUser(to);
            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        } //xóa group riêng

        private string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from} - {to}" : $"{to} - {from}";
        } // get tên group theo user nhận và user gửi

        public async Task AddUserForNoti(string user)
        {
            _chatService.AddUserForNoti(user, Context.ConnectionId);
        } // add danh sách user đăng ký nhận noti
        #endregion

    }
}
