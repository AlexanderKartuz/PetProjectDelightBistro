using MazeCore.Cells.Interfaces;
using MazeCore.Characters;
using MazeCore.Interfaces;

namespace MazeCore
{
    public class Maze : IMaze
    {
        public IInputReader InputReader { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Hero Hero { get; set; }

        public int Seed { get; set; }
        public MazeFogOfWar FogOfWar { get; set; } = new MazeFogOfWar();
        // ground == '.'
        // wall '#'
        // coin 'c'
        public IList<IBaseCell> Surface { get; set; } = new List<IBaseCell>();

        public IList<string> EventHistory { get; set; } = new List<string>();
        public bool IsSecretMaze { get; set; } = false;


        public IBaseCell? this[int x, int y]
        {
            get
            {
                //var numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

                //numbers.First(number => number > 5); // => 6
                //numbers.First(number => number > 12); // !!! EXCEPTION !!!

                //numbers.FirstOrDefault(number => number > 5); // => 6
                //numbers.FirstOrDefault(number => number > 12); // 0 DEFAULT NULL

                //numbers.Single(number => number > 5); // !!! EXCEPTION !!!
                //numbers.Single(number => number > 7); // => 8 
                //numbers.Single(number => number > 12); // !!! EXCEPTION !!!

                //numbers.SingleOrDefault(number => number > 5); // !!! EXCEPTION !!!
                //numbers.SingleOrDefault(number => number > 7); // => 8 
                //numbers.SingleOrDefault(number => number > 12); // => 0 DEFAULT NULL

                return Surface.SingleOrDefault(cell => cell.X == x && cell.Y == y);
            }
        }
    }
}
