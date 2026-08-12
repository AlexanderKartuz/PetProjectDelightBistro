using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Characters
{
    public class Hero : BaseCharacter, IBaseCharacter
    {
        public Hero(IMaze maze) : base(maze)
        {
        }
        public override char Symbol => '@';
        public override bool Interaction(IBaseCharacter character)
        {
            throw new NotImplementedException();
        }

    }
}
