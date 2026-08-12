namespace FirstConsoleApp.SeaBattleHumanVsBot;

public class Ship
{ 
    public List<Cell> Coordinates { get; set; } = new List<Cell>();
    
    int Hits { get; set; }
    
    public void MarkHit(Cell cell)
    {
        cell.Ship.Hits += 1;
    }
    
    public bool IsDestroyed()
    { 
        return Hits == Coordinates.Count;
    }

    /// <summary>
    /// Make list of empty cells near ship
    /// </summary>
    public List<Cell> GetNeighboringCells(Field field)
    {
        HashSet<Cell> cells = new HashSet<Cell>();
        foreach (var cell in Coordinates)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int row = cell.Row + i;
                    int column = cell.Column + j;

                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    if (row < 0 ||
                        row >= field.Cells.GetLength(0) ||
                        column < 0 ||
                        column >= field.Cells.GetLength(1))
                    {
                        continue;
                    }
                    
                    Cell neighbor =  field.Cells[row, column];

                    if (neighbor.State == CellState.Empty)
                    {
                        cells.Add(neighbor);
                    }
                }
            }
        }
        return cells.ToList();
    }
    
}