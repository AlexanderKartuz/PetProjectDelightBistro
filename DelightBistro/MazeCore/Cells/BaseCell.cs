using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public abstract class BaseCell : IBaseCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public IMaze Maze { get; set; }
        public abstract char Symbol { get; }
        public virtual bool IsBonusCell { get; init; } = false;

        public BaseCell(IMaze maze)
        {
            Maze = maze;
        }

        public abstract bool Interaction(IBaseCharacter character);
    }
}
