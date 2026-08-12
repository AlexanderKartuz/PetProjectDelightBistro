namespace FirstConsoleApp.SeaBattleHumanVsBot;

public abstract class Player
{ 
    public Field Field { get; set; } = new Field();

    public List<Ship> Ships { get; set; } = new List<Ship>();

    public int CountOfAliveShips()
    {
        return Ships.Count(ship => !ship.IsDestroyed());
    }
    
    protected Ship PlaceShipInField(int row, int column, int size, bool isHorisontal)
    {
        var ship = new Ship();
        var cellsOfShip = new List<Cell>();
                
        for (var i = 0; i < size; i++)
        {
            if (isHorisontal)
            {
                cellsOfShip.Add(Field.Cells[row, column + i]);
                Field.Cells[row, column + i].Ship = ship;
            }
            else
            {
                cellsOfShip.Add(Field.Cells[row + i, column]);
                Field.Cells[row + i, column].Ship = ship;
            }
        }
        
        ship.Coordinates = cellsOfShip;
        Ships.Add(ship);
        foreach (var cell in cellsOfShip)
        {
            cell.State = CellState.Ship;
        }
        
        foreach (var cell in cellsOfShip)
        {
            Console.WriteLine($"Ship cell: row={cell.Row} column={cell.Column}");
        }
        
        return ship;
    }    
    public abstract bool MakeMove(Player enemy);
    public abstract void PlaceShips();

} 