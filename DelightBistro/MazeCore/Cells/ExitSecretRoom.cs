using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class ExitSecretRoom : BaseCell
    {
        public ExitSecretRoom(IMaze maze) : base(maze)
        {

        }

        public override char Symbol => 'E';
        private int _countInteraction = 0;
        private const int MAX_INTERACTION = 2;

        public override bool Interaction(IBaseCharacter character)
        {
            _countInteraction++;
            if (_countInteraction >= MAX_INTERACTION)
            {
                Maze.EventHistory.Add("You left the secret room.");

                _countInteraction = 0;

                return false;
            }
            return true;
        }
    }
}
