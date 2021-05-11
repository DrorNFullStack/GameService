namespace GameLib.Models
{
    public class Player
    {
        public int Score { get; set; }
        public string PlayerID { get; set; }
        public string Name { get; set; }
        public bool IsMyTurn { get; set; }
    }
}