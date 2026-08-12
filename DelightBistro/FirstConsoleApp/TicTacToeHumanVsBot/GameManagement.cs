namespace FirstConsoleApp.TicTacToeHumanVsBot;

public class GameManagement
{
    private TicTacToeBoard _board;
    private GameRules _rules;
    private BasePlayers _currentPlayer;
    private BasePlayers _player1;
    private BasePlayers _player2;

    public GameManagement()
    {
        _board = new TicTacToeBoard();
        _rules = new GameRules();

        _player1 = new HumanPlayer("X");
        _player2 = new BotPlayer("O");

        _currentPlayer = _player1;
    }
    public void Greeting()
    {
        Console.WriteLine("===================================");
        Console.WriteLine("       TIC-TAC-TOE GAME            ");
        Console.WriteLine("===================================");
        Console.WriteLine("\nINSTRUCTIONS:");
        Console.WriteLine("- You are X, Bot is O");
        Console.WriteLine("- Choose position 1-9");
        Console.WriteLine("- First to get 3 in a row wins!");
        Console.WriteLine("\nBoard positions:");
        _board.Draw();
    }
    public void Start()
    {

        var playRound = true;

        while (playRound)
        {
            _currentPlayer.MakeMove(_board);

            _board.MoveCount++;
            if (_currentPlayer == _player2)
            {
                _board.Draw();
            }


            var result = _rules.GetGameResult(_board, _player1.Mark, _player2.Mark);

            if (result == "player Wins")
            {
                Console.WriteLine("You Win!");
                playRound = false;
            }
            else if (result == "Bot Wins")
            {
                Console.WriteLine("Bot Wins!");
                playRound = false;
            }
            else if (result == "No winners")
            {
                Console.WriteLine("No Winners!");
                playRound = false;
            }

            if (_currentPlayer == _player1)
            {
                _currentPlayer = _player2;
            }
            else
            {
                _currentPlayer = _player1;
            }
        }
    }

    public bool IsReadyToPlayAgain()
    {
        Console.WriteLine(" Do you want to play another round? (y/n)");

        string playAnotherRound = Console.ReadLine();
        if (playAnotherRound == "y")
        {
            Reset();
            _board.Draw();
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _board.Clear();
        _currentPlayer = _player1;
    }
}
