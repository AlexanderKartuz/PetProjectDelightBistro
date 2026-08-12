using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class SkipingMove : BaseCell
    {
        public override char Symbol => 'O';

        public SkipingMove(IMaze maze) : base(maze)
        {

        }

        public override bool Interaction(IBaseCharacter character)
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("pit_sound.wav");

            Maze.EventHistory.Add("opps! yuo in pit");

            character.Hp -= 10;
            character.Coins--;

            return true;
        }

        public bool HasFallenIntoPit(IBaseCharacter character)
        {
            const int CHANCE_OUT_PERCENT = 75;
            const int MAX_VALUE_PERCENT = 100;

            var cellsWithPits = Maze
                .Surface
                .Where(pit => pit.Symbol == Symbol)
                .ToList();

            foreach (var item in cellsWithPits)
            {
                if (item.X == character.X && item.Y == character.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}