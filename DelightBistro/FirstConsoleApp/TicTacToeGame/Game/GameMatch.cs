namespace FirstConsoleApp.TicTacToeGame
{
    internal class GameMatch
    {
        public void RunMatch()
        {
            Console.WriteLine("Start match.");
            Console.WriteLine("Enter player1 name (mark - X)");
            var playerName1 = Console.ReadLine();
            Console.WriteLine("Enter player2 name (mark - O)");
            var playerName2 = Console.ReadLine();

            var player1 = new Player(playerName1, 'X');
            var player2 = new Player(playerName2, 'O');

            var playAgain = "yes";
            do
            {
                var game = new Game(player1, player2);
                game.Start();

                Console.WriteLine("\nDo you want to play again? (Yes/No):");
                playAgain = Console.ReadLine().ToLower();
            }
            while (playAgain == "yes" || playAgain == "y");

            Console.WriteLine("Statistic:");
            player1.DisplayStats();
            player2.DisplayStats();
        }
    }
}
