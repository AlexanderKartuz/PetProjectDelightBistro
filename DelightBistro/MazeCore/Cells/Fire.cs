using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Fire : BaseCell
    {
        public Fire(IMaze maze) : base(maze)
        {

        }

        public override char Symbol => '^';


        public override bool Interaction(IBaseCharacter character)
        {
            character.Hp--;
            character.Burning++;

            Maze.EventHistory.Add("You're on fire!");

            Maze.Surface.Remove(this);

            var ground = new Ground(Maze)
            {
                X = X,
                Y = Y,
            };
            Maze.Surface.Add(ground);

            return true;
        }


    }
}
