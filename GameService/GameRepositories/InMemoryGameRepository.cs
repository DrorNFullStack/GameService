using GameLib;
using GameLib.Models;
using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.GameRepositories
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Dictionary<string, BackgammonLogic> mem;
        private readonly IServiceProvider serviceProvider;
        public InMemoryGameRepository(IServiceProvider serviceProvider)
        {
            mem = new Dictionary<string, BackgammonLogic>();
            this.serviceProvider = serviceProvider;
        }

        public Task<GameView> GenerateGameAsync(string gameID)
        {
            if (!mem.ContainsKey(gameID))
            {
                BackgammonLogic game = (BackgammonLogic)serviceProvider.GetService(typeof(BackgammonLogic));
                mem.Add(gameID, game);
                return Task.FromResult(new GameView
                {
                    Triangles = game.Board.Triangles.Values.OrderBy(t => t.Position),
                    Bar = game.Board.Bar.Pieces,
                    SafePieces = game.Board.SafePieces
                });
            }
            return null;

        }
    }
}
