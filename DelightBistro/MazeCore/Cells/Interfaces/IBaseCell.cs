using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells.Interfaces
{
    public interface IBaseCell
    {
        bool IsBonusCell { get; init; }
        IMaze Maze { get; set; }
        char Symbol { get; }
        int X { get; set; }
        int Y { get; set; }

        bool Interaction(IBaseCharacter character);
    }
}