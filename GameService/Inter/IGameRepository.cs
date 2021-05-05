using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Inter
{
    public interface IGameRepository
    {
        public Task<Game> CreateGameAsync();
        public Task<Game> CloseGameAsync(Guid id);
        public Task<Game> GetGameAsync(Guid id);
        
    }
}
