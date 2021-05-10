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
        Task<bool> AddLiveUser(User user);
        Task<bool> RemoveLiveUser(string userID);
    }
}
