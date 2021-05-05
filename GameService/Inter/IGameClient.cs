using GameLib.Models;
using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Inter
{
    public interface IGameClient
    {
        Task GameCreatedAsync(Game game);
    }
}
