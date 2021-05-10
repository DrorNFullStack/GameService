using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.OnlineUserManager
{
    public class InMemoryOnlineUserManager : IOnlineUserManager

    {
        readonly Dictionary<string, User> dic = new Dictionary<string, User>();

        public Task<bool> AddLiveUser(User user)
        {
            return Task.Run(() => dic.TryAdd(user.UserID, user));
        }

        public Task<IEnumerable<User>> GetLiveUsers()
        {
            return Task.Run(() => dic.Values.Select(k => k));
        }

        public Task<bool> RemoveLiveUser(string userID)
        {
            return Task.Run(() => dic.Remove(userID));
        }
    }
}
