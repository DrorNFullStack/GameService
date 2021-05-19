namespace GameLib.Models
{
    public class GameAction
    {
        public int StartingPosition { get; set; }
        public int DestinationPosition { get; set; }
        public DiceResult RelevantRoll { get; set; }
    }
}
