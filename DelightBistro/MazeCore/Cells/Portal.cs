using MazeCore.Cells.Interfaces;
using MazeCore.Characters.Interfaces;
using MazeCore.Interfaces;

namespace MazeCore.Cells
{
    public class Portal : BaseCell
    {
        private readonly IInputReader _inputReader;
        public override char Symbol => '%';
        public override bool IsBonusCell { get; init; } = true;
        public Portal LinkedPortal { get; set; }
        public bool IsSingleUse {  get; set; }

        public Portal(IMaze maze, IInputReader inputReader) : base(maze)
        {
            _inputReader = inputReader;
        }

        public override bool Interaction(IBaseCharacter character)
        {
            Console.WriteLine("A portal stands here. Use it? (Y/N)");
            
            var key = _inputReader.ReadKey();
            if (key != ConsoleKey.Y)
            {
                MazeSoundPlayer soundPlayer = new MazeSoundPlayer();
                soundPlayer.PlayMusic("portal_sound.wav");

                Maze.EventHistory.Add("You decided not to use the portal");
                return true;
            }

            character.X = LinkedPortal.X;
            character.Y = LinkedPortal.Y;

            Maze.EventHistory.Add("You stepped through the portal");

            if (IsSingleUse)
            {
                RemovePortals();
            }

            return false;
        }

        private void RemovePortals()
        {
            Maze.Surface.Remove(this);
            Maze.Surface.Remove(LinkedPortal);

            var groundOnEntrancePortal = new Ground(Maze)
            {
                X = X,
                Y = Y,
            };

            var groundOnDestinationPortal = new Ground(Maze)
            {
                X = LinkedPortal.X,
                Y = LinkedPortal.Y,
            };

            Maze.Surface.Add(groundOnEntrancePortal);
            Maze.Surface.Add(groundOnDestinationPortal);
        }
    }
}
