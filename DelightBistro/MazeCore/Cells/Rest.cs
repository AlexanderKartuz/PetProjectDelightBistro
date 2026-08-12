using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Rest : BaseCell
    {


        public Rest(IMaze maze) : base(maze)
        {
        }

        public override char Symbol => 'Q';
        public override bool IsBonusCell { get; init; } = true;


        public override bool Interaction(IBaseCharacter character)
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("rest_sound.wav");

            character.Hp++;
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