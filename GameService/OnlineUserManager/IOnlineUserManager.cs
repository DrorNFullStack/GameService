using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.OnlineUserManager
{
    public interface IOnlineUserManager
    {
        Task<IEnumerable<User>> GetLiveUsers();
        Task<IEnumerable<User>> GetAvailabeUsers();
        Task<bool> AddLiveUser(User user);
        Task<User> RemoveLiveUser(string userID);
        Task UpdateUserGame(string connectionId, Guid gameID);
        Task<IEnumerable<User>> GetUnavailbeUsers();
        Task<bool> IsUserConnected(string userIdentifier);
        Task<bool> IsUserAvailable(string userID);
    }
}
