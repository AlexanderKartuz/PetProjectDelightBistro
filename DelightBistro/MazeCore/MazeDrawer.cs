using MazeCore.Cells.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore
{
    public class MazeDrawer
    {
        private IMaze _maze;
        private const int EVENT_HISTORY_LENGTH = 5;

        public void Draw(IMaze maze)
        {
            _maze = maze;
            Console.Clear();

            for (int y = 0; y < maze.Height; y++)
            {
                Console.WriteLine();

                for (int x = 0; x < maze.Width; x++)
                {
                    IBaseCell currentCell;

                    if (maze.Hero.X == x && maze.Hero.Y == y)
                    {
                        currentCell = maze.Hero;
                    }
                    else
                    {
                        currentCell = maze
                           .Surface
                           .First(cell => cell.X == x && cell.Y == y);
                    }
                    var symbol = (Math.Abs(maze.Hero.X - currentCell.X) > maze.FogOfWar.HorizontalRadius
                        || Math.Abs(maze.Hero.Y - currentCell.Y) > maze.FogOfWar.VerticalRadius)
                        ? maze.FogOfWar.Symbol : currentCell.Symbol;

                    Console.Write(symbol);
                }
            }

            Console.WriteLine();

            DrawHeroStats();
            DrawEventHsitory();
            DrawSeed();
        }

        private void DrawSeed()
        {
            Console.WriteLine($"Seed {_maze.Seed}");
        }

        private void DrawHeroStats()
        {
            var hero = _maze.Hero;
            Console.WriteLine($"Hp: {hero.Hp} Coins: {hero.Coins} Keys: {hero.Keys} SuperPower: {hero.SuperPower} Speed: {hero.Speed}");
        }

        private void DrawEventHsitory()
        {
            var messages = _maze
                .EventHistory
                .TakeLast(EVENT_HISTORY_LENGTH);

            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
