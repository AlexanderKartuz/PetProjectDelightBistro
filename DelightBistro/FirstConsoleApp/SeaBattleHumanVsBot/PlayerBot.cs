namespace FirstConsoleApp.SeaBattleHumanVsBot;

public class PlayerBot : Player
{
    private List<Cell> _hitCellsInEnemysField = new List<Cell>();
    private List<Cell> _untryedToMoveCells = new List<Cell>();
    private Random _random = new Random();

    private List<Cell> _availableCellsToPlace = new List<Cell>();

    public override bool MakeMove(Player enemy)
    {
        // Инициализируем один раз клетки в которые есть смысл стрелять
        if (_untryedToMoveCells.Count == 0)
        {
            for (var r = 0; r < 10; r++)
            {
                for (var c = 0; c < 10; c++)
                {
                    _untryedToMoveCells.Add(enemy.Field.Cells[r, c]);
                }
            }
        }
        
        _untryedToMoveCells.RemoveAll(c => c.State != CellState.Empty && c.State != CellState.Ship);
        
        var row = 0;
        var column = 0;
        if (_hitCellsInEnemysField.Count == 0) //если подстреленных кораблей нет
        {
            int index = _random.Next(_untryedToMoveCells.Count);
            row = _untryedToMoveCells[index].Row;
            column = _untryedToMoveCells[index].Column;
        }
        else if (_hitCellsInEnemysField.Count == 1) //если подстрелена только одна клетка корабля
        {
            var possibleCells = new List<Cell>();
            row = _hitCellsInEnemysField[0].Row;
            column = _hitCellsInEnemysField[0].Column;

            if (row + 1 < 10)
            {
                if (enemy.Field.Cells[row + 1, column].State == CellState.Empty
                    || enemy.Field.Cells[row + 1, column].State == CellState.Ship)
                {
                    possibleCells.Add(enemy.Field.Cells[row + 1, column]);
                }
            }
            if (row - 1 >= 0)
            {
                if (enemy.Field.Cells[row - 1, column].State == CellState.Empty
                    || enemy.Field.Cells[row - 1, column].State == CellState.Ship)
                {
                    possibleCells.Add(enemy.Field.Cells[row - 1, column]);
                }
            }
            if (column + 1 < 10)
            {
                if (enemy.Field.Cells[row, column + 1].State == CellState.Empty
                    || enemy.Field.Cells[row, column + 1].State == CellState.Ship)
                {
                    possibleCells.Add(enemy.Field.Cells[row, column + 1]);
                }
            }
            if (column - 1 >= 0)
            {
                if (enemy.Field.Cells[row, column - 1].State == CellState.Empty
                    || enemy.Field.Cells[row, column - 1].State == CellState.Ship)
                {
                    possibleCells.Add(enemy.Field.Cells[row, column - 1]);
                }
            }
            
            int indexOfCellToShoot = _random.Next(possibleCells.Count);
            
            row = possibleCells[indexOfCellToShoot].Row;
            column = possibleCells[indexOfCellToShoot].Column;
        }
        else //если подстрелено 2 и больше
        {
            var possibleCellsToShoot = new List<Cell>();
            
            //подбит по горизонтали
            if (_hitCellsInEnemysField[0].Row == _hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Row)
            {
                //для того чтобы первая клетка находилась выше последней на поле)
                if (_hitCellsInEnemysField[0].Column > _hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Column)
                {
                    _hitCellsInEnemysField.Reverse();
                }
                
                row = _hitCellsInEnemysField[0].Row;
                var colFirst = _hitCellsInEnemysField[0].Column;
                var colLast = _hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Column;
                
                if (colFirst - 1 >= 0)
                {
                    if (enemy.Field.Cells[row, colFirst - 1].State == CellState.Empty
                        || enemy.Field.Cells[row, colFirst - 1].State == CellState.Ship)
                    {
                        possibleCellsToShoot.Add(enemy.Field.Cells[row, colFirst - 1]);
                    }
                }
                if (colLast + 1 < 10)
                {
                    if (enemy.Field.Cells[row, colLast + 1].State == CellState.Empty
                        || enemy.Field.Cells[row, colLast + 1].State == CellState.Ship)
                    {
                        possibleCellsToShoot.Add(enemy.Field.Cells[row, colLast + 1]);
                    }
                }
            }

            //подбит по вертикали
            if (_hitCellsInEnemysField[0].Column == _hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Column)
            {
                //для того чтобы первая клетка находилась левее последней клетки на поле)
                if (_hitCellsInEnemysField[0].Row > _hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Row)
                {
                    _hitCellsInEnemysField.Reverse();
                }
                
                column = _hitCellsInEnemysField[0].Column;
                var rowFirst = _hitCellsInEnemysField[0].Row;
                var rowLast = _hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Row;

                if (rowFirst - 1 >= 0)
                {
                    if (enemy.Field.Cells[rowFirst - 1, column].State == CellState.Empty
                        || enemy.Field.Cells[rowFirst - 1, column].State == CellState.Ship)
                    {
                        possibleCellsToShoot.Add(enemy.Field.Cells[rowFirst - 1, column]);
                    }
                }
                if (_hitCellsInEnemysField[_hitCellsInEnemysField.Count - 1].Row + 1 < 10)
                {
                    if (enemy.Field.Cells[rowLast + 1, column].State == CellState.Empty
                        || enemy.Field.Cells[rowLast + 1, column].State == CellState.Ship)
                    {
                        possibleCellsToShoot.Add(enemy.Field.Cells[rowLast + 1, column]);
                    }
                }

            }

            int indexOfCellToShoot = _random.Next(possibleCellsToShoot.Count);
            
            row = possibleCellsToShoot[indexOfCellToShoot].Row;
            column = possibleCellsToShoot[indexOfCellToShoot].Column;
        }

        var chosenCell = enemy.Field.Cells[row, column];
        
        var shotState = enemy.Field.Shot(chosenCell);

        Console.WriteLine();
        Console.WriteLine($"Bot chosen cell: {MakeCharFromInt(column)} {row+1}");
        
        var resultOfMove = false;
        if (shotState == ShotState.Miss)
        {
            resultOfMove = false;
            Console.WriteLine("Bot missed");
            Console.WriteLine("Bots turn is over");
        }
        if (shotState == ShotState.Damaged)
        {
            _hitCellsInEnemysField.Add(chosenCell);
            resultOfMove = true;
            Console.WriteLine("Bot damaged your ship!");
            Console.WriteLine("Bot turn continues");
        }

        if (shotState == ShotState.Destroy)
        {
            _hitCellsInEnemysField.Clear();
            resultOfMove = true;
            Console.WriteLine("Bot destroyed your ship!");
            Console.WriteLine("Bots turn continues");
        }
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
        return resultOfMove;
    }

    public override void PlaceShips()
    {
        FillCells(_availableCellsToPlace);
        
        //сначала создаем все корабли
        var shipSizes = new List<int>() {4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        foreach (var size in shipSizes)
        {
            do
            {
                ///рандом координаты
                var row = 0;
                var column = 0;
                int index = _random.Next(_availableCellsToPlace.Count);
                row = _availableCellsToPlace[index].Row;
                column = _availableCellsToPlace[index].Column;
                
                //выбранная клетка для того, чтобы попробовать вставить в нее корабль
                //верхняя/левая палуба корабля

                bool isHorisontal = _random.Next(0, 2) == 1;

                if (!Field.CanPlaceShip(row, column, size, isHorisontal))
                {
                    continue;
                }

                var ship = PlaceShipInField(row, column, size, isHorisontal);
                
                
                _availableCellsToPlace.RemoveAll(c => ship.Coordinates.Contains(c));
                foreach (var neighbor in ship.GetNeighboringCells(this.Field))
                {
                    _availableCellsToPlace.Remove(neighbor);
                }
                break;
                
            } while (true);
        }
    }

    private void FillCells(List<Cell> untryedCells)
    {
        for (var row = 0; row < 10; row++)
        {
            for (var column = 0; column < 10; column++)
            {
                untryedCells.Add(Field.Cells[row, column]);
            }
        }
    }

    private char MakeCharFromInt(int column)
    {
        switch (column)
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