namespace FirstConsoleApp.SeaBattleHumanVsBot;

public class PlayerHuman : Player
{
    /// <summary>
    /// Описывает один ход человека
    /// </summary>
    public override bool MakeMove(Player enemy)
    {
        Console.WriteLine();

        Console.WriteLine("=========================================");
        Console.WriteLine("It`s you turn!");
        Console.WriteLine("=========================================");

        Console.WriteLine();
        Console.WriteLine("COORDINATES");
        var (row, column) = GetCoordinateFromConsole();
        
        var resultOfMove = enemy.Field.Shot(enemy.Field.Cells[row, column]);

        if (resultOfMove == ShotState.Miss)
        {
            Console.WriteLine("You missed");
            Console.WriteLine("Your turn is over");
            return false;
        }
        if (resultOfMove == ShotState.Damaged)
        {
            Console.WriteLine("You damaged the enemy ship!");
            Console.WriteLine("Your turn continues");
        }
        if (resultOfMove == ShotState.Destroy)
        {
            Console.WriteLine("You destroyed the enemy ship!");
            Console.WriteLine("Your turn continues");
        }
        
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
        return true;
    }
    
    /// <summary>
    /// Размещение кораблей вначале игры
    /// </summary>
    public override void PlaceShips()
    {
        Console.WriteLine();

        Console.WriteLine("=========================================");
        Console.WriteLine("Hello! It's a sea battle game with Bot!");
        Console.WriteLine("Let's arrange your ships!");
        Console.WriteLine("=========================================");

        var shipSizes = new List<int>() {4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        foreach (var size in shipSizes)
        {
            do
            {
                Field.ShowMyFieldWhenPlaceShips();
                Console.WriteLine($"SIZE - {size}");
                Console.WriteLine("DIRECTION");
                Console.WriteLine("'1' if horisontal ");
                Console.WriteLine("'0' if vertical");
                var direction = GetNumFromConsole(0, 1);
                var isHorisontal = direction == 1;

                Console.WriteLine();
                Console.WriteLine("COORDINATES");
                Console.WriteLine("Let`s enter the FIRST position of the ship");
                if (isHorisontal)
                {
                    Console.WriteLine("It`s the left side of the ship");
                }
                else
                {
                    Console.WriteLine("It`s the highest side of the ship");
                }
                
                var (row, column) = GetCoordinateFromConsole();
                Console.WriteLine("-----------------------------------------");

                if (!Field.CanPlaceShip(row, column, size, isHorisontal))
                {
                    Console.Clear();
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine("You can`t place your ship hear");
                    Console.WriteLine("Please try again");
                    Console.WriteLine("-----------------------------------------");
                    continue;
                }
                
                var ship = PlaceShipInField(row, column, size, isHorisontal);
                Console.Clear();
                break;
            } while (true);
        }
        
    }

    private (int row, int column) GetCoordinateFromConsole()
    {
        var row = 0;
        var column = 0;
        do
        {
            Console.WriteLine("Format A1");
            Console.Write("Cell: ");
            var cellCoordinate = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(cellCoordinate) || cellCoordinate.Trim().Length < 2)
            {
                Console.WriteLine("Invalid input. Try again");
                continue;
            }
            
            cellCoordinate = cellCoordinate.ToUpper().Trim();

            var columnStr = cellCoordinate[0].ToString();
            var rowStr = cellCoordinate.Substring(1);
            
            var isThisNumber = int.TryParse(rowStr, out row);

            if (!isThisNumber)
            {
                Console.WriteLine("It's not a number. Try again");
                continue;
            }
            if (row > 10 || row < 1)
            {
                Console.WriteLine("You entered an invalid number. Try again");
                continue;
            }
            
            column = IndexOfCorrectLetter(columnStr);

            if (column == -1)
            {
                Console.WriteLine("You entered an invalid letter. Try again");
                continue;
            }
            
            break;
            
        } while (true);
        
        return (row-1, column);
    }
    
    private int GetNumFromConsole(int min, int max)
    {
        var isThisANumber = false;
        var row = 0;
        do
        {
            var guessStr = Console.ReadLine();

            isThisANumber = int.TryParse(guessStr, out row);
            if (!isThisANumber)
            {
                Console.WriteLine("It's not a number. Please enter just a number");
                continue;
            }
            if (row < min || row > max)
            {
                Console.WriteLine("You entered an invalid number. Try again");
                continue;
            }
            break;
        } while (true);

        return row; 
    }

    private int IndexOfCorrectLetter(string column)
    {
        switch (column)
        {
            case "A":
                return 0;
            case "B":
                return 1;
            case "C":
                return 2;
            case "D":
                return 3;
            case "E":
                return 4;
            case "F":
                return 5;
            case "G":
                return 6;
            case "H":
                return 7;
            case "I":
                return 8;
            case "J":
                return 9;
            default:
                return -1;
        }
    }
}
