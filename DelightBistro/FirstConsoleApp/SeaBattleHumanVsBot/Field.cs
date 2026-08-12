namespace FirstConsoleApp.SeaBattleHumanVsBot;
public enum ShotState
{
    Destroy,
    Damaged,
    Miss
}

public class Field
{
    public Cell[,] Cells { get; set; } =  new Cell[10, 10];
    
    public Field()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Cells[x, y] = new Cell(x, y);
            }
        }
    }
    
    
    //координата - верхняя\левая палуба корабля
    public bool CanPlaceShip(int row, int column, int size, bool horizontal)
    {
        if (!horizontal)
        {
            if (column > 9  || column < 0) return false;//если выходит за границы столбцов

            if (row > 10 - size || row < 0) return false;
        }
        else
        {
            if (row > 9 || row < 0) return false;//если выходит за границы строк

            if (column > 10 - size || column < 0)  return false;  
            ///например: корабль длиной 4 должен начинаться
            /// максимум с 6ой клетки (6 7 8 9),
            /// тогда 9 - 4 + 1 = 6 
        }

        for (int c = 0; c < size; c++) //cделаем с каждой клеткой
        {
            var rows = 0;
            var cols = 0;
            for (int i = -1; i <= 1; i++) //в каждой ее строке
            {
                for (int j = -1; j <= 1; j++) // в каждом столбце
                {
                    if (!horizontal)
                    {
                        rows = row + i + c;
                        cols = column + j;
                    }
                    else
                    {
                        rows = row + i;
                        cols = column + j + c;
                    }
                    
                    if (rows < 0 || rows > 9 || cols < 0 || cols > 9) continue;
                    
                    Cell cell =  Cells[rows, cols];

                    if (cell.State != CellState.Empty) return false;
                }
            }
        }
        return true;
    }
    
   public ShotState Shot(Cell cellToShoot)
   {
       if (cellToShoot.State == CellState.Ship)
       {
           cellToShoot.State = CellState.Hit;
           
           cellToShoot.Ship.MarkHit(cellToShoot);

           if (cellToShoot.Ship.IsDestroyed())
           {
               var cellsNearDestroyedShip = cellToShoot.Ship.GetNeighboringCells(this);
               MakeNeighbordingCellsMiss(cellsNearDestroyedShip);
               
               return ShotState.Destroy;
           }

           return ShotState.Damaged;
       }

       cellToShoot.State = CellState.Miss;
       return ShotState.Miss;
   }

   public void MakeNeighbordingCellsMiss(List<Cell> cells)
   {
       foreach (var cell in cells)
       {
           cell.State = CellState.Miss;
       }
   }

   public void ShowMyFieldWhenPlaceShips()
   {
       Console.Write("   ");
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
               var cell = Cells[i - 1, j];

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
           Console.WriteLine();
       }
   }
   
   
   //есть и в классе SeaBattkeGame
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