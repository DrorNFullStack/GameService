using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.GameRepositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        Dictionary<Guid, Game> _mem = new Dictionary<Guid, Game>();
        public Task CloseGameAsync(Guid id)
        {
            return Task.FromResult(_mem.Remove(id));
        }

        public async Task<Game> GenerateGameAsync()
        {
            Game game = new Game();
            game.GameID = Guid.NewGuid();
            _mem.Add(game.GameID, game);
            return game;
        }

        public async Task<Game> GetGameAsync(Guid id)
        {
            _mem.TryGetValue(id, out Game game);
            return game;
        }
    }
}
