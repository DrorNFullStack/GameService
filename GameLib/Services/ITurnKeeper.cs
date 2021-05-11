using GameLib.Models;

namespace GameLib.Services
{
    public interface ITurnKeeper
    {
        Player GetActivePlayer();
        void EndTurn(Player player);
        void AddPlayer(Player player);
    }
}
