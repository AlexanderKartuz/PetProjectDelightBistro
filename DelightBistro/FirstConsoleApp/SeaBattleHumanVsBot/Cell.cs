namespace FirstConsoleApp.SeaBattleHumanVsBot;

public enum CellState
{
    Empty,
    Ship,
    Hit,
    Miss
}

public class Cell
{
    public int Row { get; }
    
    public int Column { get; }

    public CellState State { get; set; } 
    
    // если нет корабля - null
    public Ship? Ship { get; set; }

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
        State = CellState.Empty;
    }
    
}