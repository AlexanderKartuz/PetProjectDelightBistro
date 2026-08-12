using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Mimic : BaseCell
    {
        private char _mimicSymbol;

        public Mimic(IMaze maze, char mimicSymbol) : base(maze)
        {
            _mimicSymbol = mimicSymbol;
        }

        public override char Symbol
        {
            get
            {
                return _mimicSymbol;
            }
        }

        public override bool Interaction(IBaseCharacter character)
        {
            MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
            soundPlayer.PlayMusic("mimic_sound.mp3");

            Maze.EventHistory.Add("You encounteder a mimic");
            if (character.Hp < 1)
            {
                Maze.EventHistory.Add("You are too tired (0 HP) to fight.");
                return false;
            }

            character.Hp--;
            character.EnemiesKilled++;
            character.Coins += 10;
            Maze.EventHistory.Add("You were wounded and awarded");
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
