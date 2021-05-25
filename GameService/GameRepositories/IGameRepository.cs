using GameLib;
using GameLib.Models;
using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.GameRepositories
{
    public interface IGameRepository
    {
        public Task<GameView> GenerateGameAsync(string gameID);
        Task<BackgammonLogic> GetGame(string gameID);
    }
}
