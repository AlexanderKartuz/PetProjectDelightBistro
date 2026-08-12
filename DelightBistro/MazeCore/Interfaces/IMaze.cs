using MazeCore;
using MazeCore.Cells.Interfaces;
using MazeCore.Characters;

namespace MazeCore.Interfaces
{
    public interface IMaze
    {
        IBaseCell? this[int x, int y] { get; }

        IList<string> EventHistory { get; set; }
        int Height { get; set; }
        Hero Hero { get; set; }
        int Seed { get; set; }
        MazeFogOfWar FogOfWar { get; set; }
        IList<IBaseCell> Surface { get; set; }
        int Width { get; set; }
        IInputReader InputReader { get; set; }
        bool IsSecretMaze { get; set; }

    }
}