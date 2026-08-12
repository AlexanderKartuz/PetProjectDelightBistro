using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Trap : BaseCell
    {
        public IMazeSoundPlayer SoundPlayer { get; set; }
        public Trap(IMaze maze) : base(maze)
        {
        }

        public override char Symbol => '▣';

        public override bool Interaction(IBaseCharacter character)
        {
            SoundPlayer = new MazeSoundPlayer();
            SoundPlayer.PlayMusic("trap_sound.wav");

            Maze.EventHistory.Add("Look out, it's a trap");
            character.Hp--;
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