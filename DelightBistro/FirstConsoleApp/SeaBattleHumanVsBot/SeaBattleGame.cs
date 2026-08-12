namespace FirstConsoleApp.SeaBattleHumanVsBot;

public class SeaBattleGame
{
    private bool _isGameOver = false;

    private PlayerHuman _human = new();

    private PlayerBot _bot = new();

    private Player _currentPlayer; // Bot or Human

    private Player _winner;

    public void StartGame()
    {
        _currentPlayer = _human;
        
        _human.PlaceShips();
        _bot.PlaceShips();

        Round();

        SayResults();
    }

    public void Round()
    {
        do
        {
            ShowFields(_human.Field, _bot.Field);

            Player enemy = _currentPlayer == _human ? _bot : _human;
            var IslastMoveHitLucky = _currentPlayer.MakeMove(enemy);

            ChangeTurnIfNeeded(IslastMoveHitLucky);

            _isGameOver = enemy.CountOfAliveShips() == 0;

            if (_isGameOver) _winner = _currentPlayer;
            
            Console.Clear();
            
        } while (!_isGameOver);
    }
    
    private void ShowFields(Field playerField, Field enemyField)
    {
        Console.WriteLine("Yout field:              Bot`s field:");
        Console.Write("   ");
        for (int j = 0; j < 10; j++)
        {
            Console.Write(MakeCharFromInt(j) + " ");
        }

        Console.Write("       ");
        for (int j = 0; j < 10; j++)
        {
            Console.Write(MakeCharFromInt(j) + " ");
        }

        Console.WriteLine();

        for (int i = 1; i <= 10; i++)
        {
            
            //чтобы при числе 10 (из двух цифр) не смещалась строка в матрице
            if (i == 10)
            {
                Console.Write(i + " ");
            }
            else
            {
                Console.Write(" " + i + " ");
            }
            
            for (int j = 0; j < 10; j++)
            {
                var cell = playerField.Cells[i - 1, j];

                var symbol = ' ';
                if (cell.State == CellState.Empty)
                    symbol = '~';
                if (cell.State == CellState.Hit)
                    symbol = 'x';
                if (cell.State == CellState.Miss)
                    symbol = '*';
                if (cell.State == CellState.Ship)
                    symbol = '▄';
                Console.Write(symbol + " ");
            }

            Console.Write("    ");

            //чтобы при числе 10 (из двух цифр) не смещалась строка в матрице
            if (i == 10)
            {
                Console.Write(i + " ");
            }
            else
            {
                Console.Write(" " + i + " ");
            }            
            
            for (int j = 0; j < 10; j++)
            {
                var cell = enemyField.Cells[i - 1, j];

                var symbol = ' ';
                if (cell.State == CellState.Empty)
                    symbol = '~';
                if (cell.State == CellState.Hit)
                    symbol = 'x';
                if (cell.State == CellState.Miss)
                    symbol = '*';
                if (cell.State == CellState.Ship)
                    symbol = '~';
                Console.Write(symbol + " ");
            }

            Console.WriteLine();
        }
    }

    private void ChangeTurnIfNeeded(bool lastMoveHit)
    {
        if (!lastMoveHit)
        {
            _currentPlayer = _currentPlayer == _human ? _bot : _human;
        }
    }
    
    /// <summary>
    /// итоги игры
    /// </summary>
    public void SayResults()
    {
        Console.Clear();
        if (_human == _winner)
        {
            Console.WriteLine("You are Win! My congratulations!");
            Console.WriteLine("Thanks for this game");
            Console.WriteLine("Goodbye");
        }
        else
        {
            Console.WriteLine("Bot is the winner. Thanks for this game");
            Console.WriteLine("You always can try again");
        }
    }

    private char MakeCharFromInt(int index)
    {
        switch (index)
        {
            case 0:
                return 'A';
            case 1:
                return 'B';
            case 2:
                return 'C';
            case 3:
                return 'D';
            case 4:
                return 'E';
            case 5:
                return 'F';
            case 6:
                return 'G';
            case 7:
                return 'H';
            case 8:
                return 'I';
            case 9:
                return 'J';
            default:
                return '?';
        }
    }

}