namespace FirstConsoleApp.Minesweeper
{
    public abstract class BaseMinesweeper
    {
        protected GameRule _rule = new();
        public CellInformation[,] Grid;
        private bool IsBlowUp = false;
        private int NumberOfOpenedCells = 0;

        /// <summary>
        /// This is a main method to play Minesweeper
        /// </summary>
        public void PlayMinesweeper()
        {
            ConigurateGameRule();

            CreateField();     // возможно стоит собрать в нем методы PlantMines() и NumberOfMinesAround()

            PlantMines();

            NumberOfMinesAround();

            DrawField();

            PlayOneRound();

            SayToUserTheGameResult();
        }
        /// <summary>
        /// This is a method to Draw a field in CMD
        /// </summary>
        private void DrawField()
        {
            Console.Clear();
            Console.Write("      ");
            
            for(int i = 1; i <= _rule.FieldWidth; i++)
            {
                Console.Write($"{i,-3}");
            }
            Console.WriteLine();
            Console.Write("    +");
            
            for(int i = 1; i <= _rule.FieldWidth * 3; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine();

            for (int i = 0; i < _rule.FieldHeight; i++)
            {
                Console.Write($" {i+1,-2} | ");

                for(int j = 0; j < _rule.FieldWidth; j++)
                {
                    if (Grid[i,j].IsCellOpened)
                    {
                        if (Grid[i, j].ValueOfCells == -1)
                        {
                            Console.Write("*  ");
                        }
                        else
                        {
                            Console.Write($"{Grid[i, j].ValueOfCells, -3}");
                        }
                    }
                    else
                    {
                        Console.Write("?  ");
                    }
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// This is a method for telling a player the game results
        /// </summary>
        private void SayToUserTheGameResult()
        {
            if (NumberOfOpenedCells == (_rule.FieldWidth * _rule.FieldHeight - _rule.NumberOfBombs))
            {
                Console.WriteLine("You are winner");
            }
            else
            {
                Console.WriteLine("Looser");
            }

            Console.WriteLine("The end");
        }
        /// <summary>
        /// This is a method to basic game manipulations, calling basic game mechanics
        /// </summary>
        private void PlayOneRound()
        {

            do
            {
                var (Height, Width) = ChooseCell("Enter cordinats of cell");
                IsBlowUp = OpenCell(Height, Width);
                Console.WriteLine($"height:{Height} width: {Width} BlowUp: {IsBlowUp}");
                DrawField();
                NumberOfOpenedCells = 0;
                for (int i = 0; i < _rule.FieldHeight; i++)
                    for(int j = 0; j < _rule.FieldWidth; j++)
                    {
                        if (Grid[i,j].IsCellOpened)
                        {
                            NumberOfOpenedCells++;
                        }
                    }

            } while (!IsBlowUp && NumberOfOpenedCells < (_rule.FieldWidth * _rule.FieldHeight - _rule.NumberOfBombs)); 
        }
        /// <summary>
        /// This is a method to OpenCell. variable IsCellOpened - True
        /// </summary>
        private bool OpenCell(int x, int y)
        {

            if (Grid[x, y].IsCellOpened)
            {
                return false;
            }

            Grid[x, y].IsCellOpened = true;

            if(Grid[x, y].ValueOfCells == -1) // lose
            {
                
                return true;
            }
            if(Grid[x, y].ValueOfCells == 0)
            {
                for(int i = -1; i <= 1; i++)
                    for(int j = -1; j <= 1; j++)
                    {
                        if(i == 0 && j == 0)
                        {
                            continue;
                        }
                        if (x + i >= 0 && x + i < _rule.FieldHeight && y + j >= 0 && y + j < _rule.FieldWidth)
                        {
                            if (!Grid[x+i,y+j].IsCellOpened)
                            OpenCell(x + i, y + j);
                        }
                    }
            }
            return false;
        }
        /// <summary>
        /// This is a method to create Field - two-dimensional array
        /// </summary>
        private void CreateField()
        {

            Grid = new CellInformation[_rule.FieldHeight, _rule.FieldWidth];
            for (int i = 0; i < _rule.FieldHeight; i++)
                for (int j = 0; j < _rule.FieldWidth; j++)
                {
                    Grid[i, j] = new CellInformation();
                }
        }
        /// <summary>
        /// This is a method to randomly plant mines on a field. ValueOfCells = -1
        /// </summary>
        private void PlantMines()
        {

            Random rnd = new Random();
            int planted = 0;

            while (planted < _rule.NumberOfBombs)
            {
                int i = rnd.Next(0, _rule.FieldHeight);
                int j = rnd.Next(0, _rule.FieldWidth);

                if (Grid[i, j].ValueOfCells != -1)
                {
                    Grid[i, j].ValueOfCells = -1;
                    planted++;
                }
            }
        }
        /// <summary>
        /// This is a method to count mines around opened cell
        /// </summary>
        private void NumberOfMinesAround()
        {
            for (int i = 0; i < _rule.FieldHeight; i++)
                for(int j = 0; j < _rule.FieldWidth; j++)
                {
                    if (Grid[i, j].ValueOfCells != -1)
                    {
                        var MinesAround = 0;

                        for (int x = i-1; x <= i+1; x++)
                            for (int y = j-1; y <= j+1; y++)
                            {
                                if (x < 0 || x >= _rule.FieldHeight || y < 0 || y >= _rule.FieldWidth)
                                {
                                    continue;
                                }
                                if (Grid[x, y].ValueOfCells == -1)
                                {
                                    MinesAround++;
                                }
                            }
                        Grid[i, j].ValueOfCells = MinesAround;
                    }
                }

        }
        /// <summary>
        /// This is a method to configurate game rules
        /// </summary>
        protected abstract void ConigurateGameRule();

        /// <summary>
        /// This is a abstract method for entering cell coordinate values ​​from the CMD
        /// </summary>
        protected (int,int) ChooseCell(string messageForUser)
        {
            var isThisANumberForHeight = false;
            var isThisANumberForWidth = false;
            var Height = 0;
            var Width = 0;
            do
            {
                Console.WriteLine(messageForUser);
                Console.WriteLine("Enter x(Height):");
                var x = Console.ReadLine();
                Console.WriteLine("Enter y(Width):");
                var y = Console.ReadLine();

                isThisANumberForHeight = int.TryParse(x,out Height);
                isThisANumberForWidth = int.TryParse(y, out Width);
                if (!isThisANumberForHeight || Height < 1 || Height > _rule.FieldHeight)
                {
                    Console.WriteLine("x is not a number or cell don't exist. Please enter coordinat of correct cell");
                }
                if (!isThisANumberForWidth || Width < 1 || Width > _rule.FieldWidth)
                {
                    Console.WriteLine("y is not a number  or cell don't exist. Please enter coordinat of correct cell");
                }

            } while (!isThisANumberForHeight);

            return (Height - 1, Width - 1);
        }
    }
}
