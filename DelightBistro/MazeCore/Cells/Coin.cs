using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Coin : BaseCell
    {
        public const char SYMBOL = 'c';
        public override bool IsBonusCell { get; init; } = true;

        public Coin(IMaze maze) : base(maze)
        {
        }

        public override char Symbol => SYMBOL;

        public override bool Interaction(IBaseCharacter character)
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("coin_sound.mp3");

            Maze.EventHistory.Add("Look, it's a coin");

            character.Coins++;

            Replace();

            return true;
        }

        private void Replace()
        {
            Maze.Surface.Remove(this);
            var ground = new Ground(Maze)
            {
                X = X,
                Y = Y,
            };
            Maze.Surface.Add(ground);
        }
    }
}