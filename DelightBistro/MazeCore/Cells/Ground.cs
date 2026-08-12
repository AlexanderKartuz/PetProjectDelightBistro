using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Ground : BaseCell
    {
        public Ground(IMaze maze) : base(maze)
        {
        }

        //public override char Symbol
        //{
        //    get
        //    {
        //        return '.';
        //    }
        //}
        public override char Symbol => '░';

        public override bool Interaction(IBaseCharacter character)
        {
            return true;
        }
    }
}
