using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    internal class Lava : BaseCell
    {
        public Lava(IMaze maze) : base(maze)
        {
        }

        public override char Symbol => 'L';

        public override bool Interaction(IBaseCharacter character)
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("lava_sound.wav");

            Maze.EventHistory.Add("This 'hot pot' too hot for you.");
            character.Hp--;
            character.Burning += 2;

            return true;
        }
    }
}