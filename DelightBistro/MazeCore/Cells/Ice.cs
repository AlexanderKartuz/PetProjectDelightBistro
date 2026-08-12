using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Ice : BaseCell
    {
        public override char Symbol => 'i';
        public Ice(IMaze maze) : base(maze)
        {
        }

        public override bool Interaction(IBaseCharacter character)
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("ice_sound.wav");

            character.Hp -= 1; // Need a new method in Character for logic. (Other cells can change hp, speed)
            character.Speed -= 1;

            Maze.EventHistory.Add("You are freezing!");

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