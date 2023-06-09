namespace EmployeeManagementBE.SignalR.Services
{
    public class ChatService
    {
        private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> Users_Noti = new Dictionary<string, string>();

        public void AddUserForNoti(string user, string connectionId)
        {
            lock (Users_Noti)
            {
                if (Users_Noti.ContainsKey(user))
                {
                    Users_Noti[user] = connectionId;
                }
                else
                {
                    Users_Noti.Add(user, connectionId);
                }
            }
        }

        public string GetNotiConnectionIdByUser(string user)
        {
            lock (Users_Noti)
            {
                return Users_Noti.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }
        public string GetUserNotiByConnectionId(string conn)
        {
            lock (Users_Noti)
            {
                return Users_Noti.Where(x => x.Value == conn).Select(x => x.Key).FirstOrDefault();
            }
        }
        public void RemoveUserNoti(string user)
        {
            lock (Users_Noti)
            {
                if (Users_Noti.ContainsKey(user))
                {
                    Users_Noti.Remove(user);
                }
            }
        }

        public bool AddUserToList(string userToAdd)
        {
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if (user.Key.ToLower() == userToAdd.ToLower())
                    {
                        return false;
                    }
                }

                Users.Add(userToAdd, "");
                return true;
            }
        }
        public void AddUserConnectionId(string user, string connectionId)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                return Users.Where(s => s.Value == connectionId).Select(s => s.Key).FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }


        public void RemoveUserFromList(string user)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users.Remove(user);
                }
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(s => s.Key).Select(s => s.Key).ToArray();
            }
        }

    }
}
