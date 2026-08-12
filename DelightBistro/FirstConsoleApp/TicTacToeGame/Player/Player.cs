namespace FirstConsoleApp.TicTacToeGame
{
    internal class Player
    {
        public string Name { get; set; }
        public char Mark { get; set; }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;

        public Player(string userName, char userMark)
        {
            Name = userName;
            Mark = userMark;
        }
        // Get coordinates from console
        public void GetMove(out int row, out int column)
        {
            while (true)
            {
                Console.WriteLine($"{Name} ({Mark}), enter row and column between 1 and 3, use SPACE to split.");
                string input = Console.ReadLine();
                string[] coordinates = input.Split();

                if (coordinates != null && coordinates.Length == 2 && int.TryParse(coordinates[0], out row) && int.TryParse(coordinates[1], out column)
                    && row >= 1 && row <= 3 && column >= 1 && column <= 3)
                {
                    break;
                }
                Console.WriteLine("Error! Wrong coordinates. Enter values between 1 and 3, split it with SPACE");
            }
        }
        public void CountWins()
        {
            Wins++;
        }
        public void CountLosses()
        {
            Losses++;
        }
        public void CountDraws()
        {
            Draws++;
        }
        public void DisplayStats()
        {
            Console.WriteLine($"{Name} - Wins: {Wins}, Losses: {Losses}, Draws {Draws}");
        }
    }
}
