namespace FirstConsoleApp.TicTacToeGame
{
    internal class Game
    {
        private Board board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public Game(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
            board = new Board();
            currentPlayer = player1;
        }
        public void Start()
        {
            Console.WriteLine("Star the game.");
            Console.WriteLine($"{player1.Name}, used {player1.Mark} VS {player2.Name}, used {player2.Mark}");
            while (true)
            {
                board.ShowBoard();
                var row = -1;
                var column = -1;
                currentPlayer.GetMove(out row, out column);

                if (!board.SetField(row, column, currentPlayer.Mark))
                {
                    continue;
                }

                if (board.CheckWinner(currentPlayer.Mark))
                {
                    board.ShowBoard();
                    Console.WriteLine($"Congratulations, {currentPlayer.Name} is winner!");

                    currentPlayer.CountWins();
                    Player loser = currentPlayer == player1 ? player2 : player1;
                    loser.CountLosses();

                    break;
                }
                if (board.IsBoardFull())
                {
                    board.ShowBoard();
                    Console.WriteLine("A draw");
                    player1.CountDraws();
                    player2.CountDraws();
                    break;
                }
                if (currentPlayer == player1)
                {
                    currentPlayer = player2;
                }
                else
                {
                    currentPlayer = player1;
                }
            }
        }
    }
}
