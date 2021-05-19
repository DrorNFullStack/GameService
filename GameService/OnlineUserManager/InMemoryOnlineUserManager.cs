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

        public async Task<IEnumerable<User>> GetAvailbeUsers()
        {
            var users = await GetLiveUsers();
            return users.Where(u => u.GameID.Equals(Guid.Empty));
        }

        public Task<IEnumerable<User>> GetLiveUsers()
        {
            return Task.Run(() => dic.Values.Select(k => k));
        }

        public async Task<IEnumerable<User>> GetUnavailbeUsers()
        {
            var users = await GetLiveUsers();
            return users.Where(u => !u.GameID.Equals(Guid.Empty));
        }

        public Task<User> RemoveLiveUser(string userID)
        {
            return Task.Run(() =>
            {
                var user = dic[userID];
                dic.Remove(userID);
                return user;
            });
        }

        public Task UpdateUserGame(string connectionId, Guid gameID) =>
            Task.Run(() =>
                        { if (dic.TryGetValue(connectionId, out User user)) user.GameID = gameID; });

    }
}
