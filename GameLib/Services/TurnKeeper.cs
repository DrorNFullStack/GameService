using GameLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.Services
{
    public class TurnKeeper : ITurnKeeper
    {
        public LinkedList<Player> Players { get; }

        private LinkedListNode<Player> activePlayer;
        public TurnKeeper() => Players = new LinkedList<Player>();
        public void AddPlayer(Player player)
        {
            var node = Players.AddLast(player);
            if (Players.Count < 2)
                activePlayer = node;
        }

        public void EndTurn(Player player) => activePlayer = activePlayer.Next ?? Players.First;

        public Player GetActivePlayer() => activePlayer.Value;
    }
}
