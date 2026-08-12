namespace FirstConsoleApp.BattleShipGame
{
    public class GameRule
    {
        public int[,] FirstPlayerGameField { get; set; }
        public int[,] SecondPlayerGameField { get; set; }
        public int[,] CurrentMatrix { get; set; }
        public int Attempt { get; set; }
        public int NumberOfRowsInGameField { get; set; }
        public int NumberOfColumnsInGameField { get; set; }

    }
}
