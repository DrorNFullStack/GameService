using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.GameRepositories
{
    public interface IGameRepository
    {
        public Task<Game> GenerateGameAsync();
        public Task CloseGameAsync(Guid id);
        public Task<Game> GetGameAsync(Guid id);
    }
}
